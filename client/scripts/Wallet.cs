
using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;
using WalletConnectSharp.Network.Models;
using Nethereum.Hex.HexConvertors.Extensions;

public partial class Wallet : Node
{
  AuthClient auth;

  WalletConnectSignClient client;

  SessionStruct session;

  public string SelectedAccount
  {
    get
    {
      if (session.Topic.IsNullOrEmpty())
      {
        return "";
      }

      return session.Namespaces["eip155"].Accounts[0];
    }
  }


  public string Account
  {
    get
    {
      string address = SelectedAccount;

      var accountSplit = address.Split(":");

      return accountSplit[^1];
    }
  }

  [RpcMethodAttribute("personal_sign")]
  class PersonalSignRequest : List<string> { }

  [Signal]
  public delegate void WalletQRCodeEventHandler(Variant value);

  [Signal]
  public delegate void WalletConnectedEventHandler();

  const string Rpc_URI = "https://rpc-mumbai.maticvigil.com/";

  public override void _Ready()
  {
    auth = (AuthClient)GetNode("../AuthClient");
  }

  public async Task<ConnectedData> StartWalletConnect()
  {
    var options = new SignClientOptions()
    {
      ProjectId = "31055fdb9030439bf05e43df2957c0bb",
      Metadata = new WalletConnectSharp.Core.Models.Pairing.Metadata()
      {
        Description = "game description",
        Icons = new[] { "https://walletconnect.com/meta/favicon.ico" },
        Name = "MMO",
        Url = "localhost"
      },
      Storage = new InMemoryStorage()
    };

    client = await WalletConnectSignClient.Init(options);

    client.Events.ListenFor(EngineEvents.SessionDelete, () =>
    {
      GD.Print("Session deleted");

      session.Topic = null;
    });

    return await client.Connect(new ConnectOptions()
    {
      RequiredNamespaces = new RequiredNamespaces()
      {
        {
          "eip155", new RequiredNamespace()
          {
            Methods = new []
            {
              "eth_sendTransaction",
              "eth_signTransaction",
              "eth_sign",
              "personal_sign",
              "eth_signTypedData",
            },
            Chains = new []
            {
              "eip155:1"
            },
            Events = new[]
            {
              "chainChanged",
              "accountsChanged"
            }
          }
        }
      }
    });
  }

  public async void ConnectToWallet()
  {
    try
    {
      var connectedData = await StartWalletConnect();

      var uri = connectedData.Uri;

      GD.Print(uri);

      CallDeferred("emit_signal", "WalletQRCode", Variant.CreateFrom(uri));

      GD.Print("Waiting approval");

      session = await connectedData.Approval;

      GD.Print("Approved");

      CallDeferred("emit_signal", "WalletConnected");
    }
    catch (Exception e)
    {
      GD.Print("Wallet connect failed: ", e);
    }
  }

  public async void Authenticate(Control target)
  {
    try
    {
      string signature = await RequestSignature(target);

      var connected = await auth.Connect(Account, signature);

      if (connected)
      {
        GD.Print("Connected to auth server!");
        target.CallDeferred("emit_signal", "_on_authenticated");
      }
    }
    catch (Exception e)
    {
      GD.Print("Failed to authenticate: ", e);
    }
  }

  private async Task<string> RequestSignature(Control target)
  {
    GD.Print("account: ", Account);

    var signature = await client.Request<PersonalSignRequest, string>(session.Topic, new PersonalSignRequest() {
      HexStringUTF8ConvertorExtensions.ToHexUTF8("1"),
      SelectedAccount
    });

    GD.Print("signature: ", signature);

    return signature;
  }
}
