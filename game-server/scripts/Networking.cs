using GameServer.scripts;
using Godot;
using System;

public partial class Networking : Node3D
{
	ENetMultiplayerPeer multiplayerPeer;

	MultiplayerSpawner spawner;

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		multiplayerPeer = new ENetMultiplayerPeer();

		Multiplayer.PeerConnected += _PeerConnected;
		Multiplayer.PeerDisconnected += _PeerDisconnected;

		multiplayerPeer.CreateServer(4242);

		Multiplayer.MultiplayerPeer = multiplayerPeer;
	}

	public override void _Ready()
	{
		spawner = (MultiplayerSpawner)GetNode("MultiplayerSpawner");
	}

	void _PeerConnected(long id)
	{
		GD.Print("Connected: ", id);

		using(var ctx = new ServerContext())
		{
			spawner.Spawn(id);
		}
	}

	void _PeerDisconnected(long id)
	{
		GD.Print("Disconnected: ", id);
	}
}
