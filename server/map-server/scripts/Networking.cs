using GameServer.scripts;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Networking : Node3D
{
	ENetMultiplayerPeer multiplayerPeer;

	MultiplayerSpawner spawner;

	Dictionary<long, Peer> peers = new();

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		GD.Print("Trying to connect");
		multiplayerPeer = new ENetMultiplayerPeer();

		Multiplayer.PeerConnected += _PeerConnected;
		Multiplayer.PeerDisconnected += _PeerDisconnected;

		multiplayerPeer.CreateServer(4242);

		Multiplayer.MultiplayerPeer = multiplayerPeer;
		GD.Print("Deu pau");
	}

	public override void _Ready()
	{
		spawner = (MultiplayerSpawner)GetNode("MultiplayerSpawner");
	}

	void _PeerConnected(long id)
	{
		GD.Print("Connected: ", id);

		Peer peer = new Peer(id);

		peers.Add(id, peer);
	}

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void onSessionMap(Variant auth_token)
	{
		GD.Print("SessioMap");
		string token = auth_token.ToString();

		using (var db = new ServerContext())
		{
			var sessions = db.Sessions.Where(x => x.AuthToken == token);

			foreach (var session in sessions)
			{
				GD.Print("session map found");
				GD.Print(session.Id);
				GD.Print(session.AuthToken);

				spawner.Spawn(Multiplayer.GetRemoteSenderId());
			}

			// Multiplayer.MultiplayerPeer.DisconnectPeer(Multiplayer.GetRemoteSenderId());

			GD.Print(auth_token);
		}
	}

	void _PeerDisconnected(long id)
	{
		GD.Print("Disconnected: ", id);

		

		peers.Remove(id);
	}
}
