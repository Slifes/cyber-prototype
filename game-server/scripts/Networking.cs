using Godot;
using System;

public partial class Networking : Node3D
{
	ENetMultiplayerPeer multiplayerPeer;

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		multiplayerPeer = new ENetMultiplayerPeer();

		Multiplayer.PeerConnected += _PeerConnected;
		Multiplayer.PeerDisconnected += _PeerDisconnected;

		multiplayerPeer.CreateServer(8002);

		Multiplayer.MultiplayerPeer = multiplayerPeer;
	}

	void _PeerConnected(long id)
	{
		GD.Print("Connected: ", id);

		CreatePlayer(id);
	}

	void _PeerDisconnected(long id)
	{
		GD.Print("Disconnected: ", id);
	}
	
	void CreatePlayer(long id) {
		var player = ResourceLoader.Load<PackedScene>("./Player.tscn");

		Node p = player.Instantiate();

		p.Name = id.ToString();

		GetNode("Players").AddChild(p);
	}
}
