using Godot;
using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Threading.Tasks;
using Teste.scripts.Crypto;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectSharp.Sign.Models.Engine;
using WalletConnectSharp.Storage;
using WalletConnectSharp.Network.Models;
using System.Collections.Generic;

public partial class Wallet2 : Node
{
	AuthClient auth;

	Heart heart;

	ConnectedData connectedData;

	WalletConnectSignClient client;

	bool actived;

	SessionStruct session;

	[Signal]
	public delegate void WalletQRCodeEventHandler(Variant value);

	[Signal]
	public delegate void WalletConnectedEventHandler();

	const String RPC_URI = "https://rpc-mumbai.maticvigil.com/";

	public override void _Ready()
	{
		auth = (AuthClient)GetNode("../AuthClient");
	}

	public async Task StartWalletConnect()
	{
		var options = new SignClientOptions()
		{
			ProjectId = "31055fdb9030439bf05e43df2957c0bb",
			Metadata = new Metadata()
			{
				Description = "game description",
				Icons = new[] { "https://walletconnect.com/meta/favicon.ico" },
				Name = "MMO",
				Url = "localhost"
			},
			Storage = new InMemoryStorage()
		};

		client = await WalletConnectSignClient.Init(options);

		String topic = null;

		connectedData = await client.Connect(new ConnectParams()
		{
			PairingTopic = topic,

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

	public void MintHeart(Node2D target)
	{
		// heart.MintHeart(target, w.Accounts[0]);
	}

	public async void ConnectToWallet()
	{
		try
		{
			await StartWalletConnect();

			if (!actived)
			{
				var uri = connectedData.Uri;

				GD.Print(uri);

				CallDeferred("emit_signal", "WalletQRCode", Variant.CreateFrom(uri));

			}

			GD.Print("Waiting approval");

			session = await connectedData.Approval;

			GD.Print("Approved");

			CallDeferred("emit_signal", "WalletConnected");

			GD.Print(session.Topic);

		} catch (Exception e)
		{
			GD.Print("Wallet connect failed: ", e);
		}
	}

	[RpcMethodAttribute("personal_sign")]
	class PersonalSignRequest: List<string> { }

	private async void _RequestSignature(Node2D target)
	{
		try
		{
			var account = session.Namespaces["eip155"].Accounts[0];

			var accountSplit = account.Split(":");

			GD.Print("account: ", accountSplit[accountSplit.Length - 1]);

			var signature = await client.Request<PersonalSignRequest, string>(session.Topic, new PersonalSignRequest() {
				HexStringUTF8ConvertorExtensions.ToHexUTF8("1"),
				account
			});

			GD.Print("signature: ", signature);

			var response = await auth.SendAuthRequest(accountSplit[accountSplit.Length - 1], signature);

			GD.Print("token: ", response.token);

			target.CallDeferred("emit_signal", "_on_authenticated");

		} catch(Exception e)
		{
			GD.Print("Request Signature failed", e);
		}
	}

	public void RequestSignature(Node2D target)
	{
		_RequestSignature(target);
	}
}
