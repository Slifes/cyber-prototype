using GameServer.scripts;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Networking : Node3D
{
	ENetMultiplayerPeer multiplayerPeer;

	MultiplayerSpawner spawner;

	Dictionary<long, Peer> peers = new();

	Node3D players;

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		GetTree().SetMultiplayer(new SceneMultiplayer());

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

		players = (Node3D)GetNode("Players");
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
		GD.Print("Requesting: Entering map.");

		int remoteId = Multiplayer.GetRemoteSenderId();

		string token = auth_token.ToString();

		using var db = new ServerContext();
		var sessions = db.Sessions.Where(x => x.AuthToken == token);

		if (!sessions.Any())
		{
			Multiplayer.MultiplayerPeer.DisconnectPeer(remoteId);
		}

		foreach (var session in sessions)
		{
			GD.Print("session map found");
			GD.Print(session.Id);
			GD.Print(session.AuthToken);
			GD.Print(session.CharacterId);

			spawner.Spawn(remoteId);
		}

		GD.Print(auth_token);
	}

	void _PeerDisconnected(long id)
	{
		GD.Print("Disconnected: ", id);

		players.GetNode(id.ToString()).QueueFree();

		peers.Remove(id);
	}
}
