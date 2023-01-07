using GameServer.scripts;
using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Networking : Node3D
{
	ENetMultiplayerPeer multiplayerPeer;

	SpawnerCustom spawner;

	WorldState worldState;

	Dictionary<long, Peer> peers = new();

	// Called when the node enters the scene tree for the first time.
	public override void _EnterTree()
	{
		GetTree().SetMultiplayer(new SceneMultiplayer());

		multiplayerPeer = new ENetMultiplayerPeer();

		Multiplayer.PeerConnected += _PeerConnected;
		Multiplayer.PeerDisconnected += _PeerDisconnected;

		multiplayerPeer.CreateServer(4242, 1000);

		Multiplayer.MultiplayerPeer = multiplayerPeer;
	}

	public override void _Ready()
	{
		spawner = GetNode<SpawnerCustom>("Players");
		worldState = GetNode<WorldState>("WorldState");
	}

	void _PeerConnected(long id)
	{
		GD.Print("Connected: ", id);

		Peer peer = new Peer(id);

		peers.Add(id, peer);
	}

	void _PeerDisconnected(long id)
	{
		GD.Print("Disconnected: ", id);

		spawner.Unspawn(id);

		peers.Remove(id);
	}

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void CheckLatency(Variant clientTime)
	{
		RpcId(Multiplayer.GetRemoteSenderId(), "ReturnLatency", clientTime);
	}

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ReturnLatency(Variant serverTime, Variant clientTime) { }

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void FetchServerTime(Variant clientTime)
	{
		double now = Time.GetUnixTimeFromSystem() * 1000.0;

		RpcId(Multiplayer.GetRemoteSenderId(), "ReturnServerTime", now, clientTime);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ReturnServerTime(Variant serverTime, Variant clientTime) { }

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void onSessionMap(Variant auth_token)
	{
		GD.Print("Requesting: Entering map.");

		int remoteId = Multiplayer.GetRemoteSenderId();

		string token = auth_token.ToString();

		/*using var db = new ServerContext();
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

			worldState.SendPlayableActor(remoteId, Variant.CreateFrom(remoteId));
		}*/

		CallDeferred("Spawn", remoteId);

		GD.Print(auth_token);
	}

	void Spawn(int remoteId)
	{
		spawner.Spawn(remoteId);

		worldState.SendPlayableActor(remoteId, Variant.CreateFrom(remoteId));
	}
}
