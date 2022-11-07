using Godot;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System;
using System.IO;
using System.Threading.Tasks;
using Teste.scripts.Crypto;
using WalletConnectSharp.Core;
using WalletConnectSharp.Core.Models;
using WalletConnectSharp.Desktop;
using WalletConnectSharp.NEthereum;

public partial class Wallet2 : Node
{
	ClientMeta meta;
	WalletConnect w;

	bool initialized;
	bool connected;
	bool oneShot;

	Web3 web3;

	Heart heart;

	[Signal]
	public delegate void WalletQRCodeEventHandler(Variant value);

	[Signal]
	public delegate void WalletConnectedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		meta = new ClientMeta()
		{
			Description = "Game Application",
			Name = "Web 3 Test",
			Icons = new[] { "https://app.warriders.com/favicon.ico" },
			URL = "https://aron.com"
		};

		w = new WalletConnect(meta);
	}

	public String URI()
	{
		return w.URI;
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!initialized)
		{
			this.EmitSignal("WalletQRCode", Variant.CreateFrom(w.URI));

			GD.Print("HUEhuehue");

			w.Connect().ContinueWith(t =>
			{
				web3 = new Web3(w.CreateProvider(new Uri("https://rpc-mumbai.maticvigil.com/")));
				GD.Print("Created Web3");

				heart = new Heart(web3);

				connected = true;
			});

			initialized = true;
		}

		if(connected && !oneShot)
		{
			this.EmitSignal("WalletConnected");
			oneShot = true;
		}
	}

	public void MintHeart(Node2D target)
	{
		heart.MintHeart(target, w.Accounts[0]);
	}

	public void RequestSignature()
	{
		w.EthSign(w.Accounts[0], "Aron beleza").ContinueWith(t =>
		{
			if (t.IsFaulted || t.IsCanceled)
			{
				GD.Print("Error");
			}
			else
			{
				if (t.Result != null)
				{
					GD.Print(t.Result);
				}
			}
		}, TaskContinuationOptions.NotOnFaulted);
	}
}
