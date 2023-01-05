using Godot;
using Godot.Collections;
using System;

partial class WorldState : Node3D
{
	Node3D players;

	Array<Variant> worldStates;

	public static double Now()
	{
		return Time.GetUnixTimeFromSystem() * 1000.0;
	}

	public override void _Ready()
	{
		players = GetNode<Node3D>("../Players");
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdatePlayersWithWorldState();
	}

	private void UpdatePlayersWithWorldState()
	{
		Dictionary dic = new();

		var playerCount = players.GetChildCount();

		var timestamp = Time.GetUnixTimeFromSystem() * 1000.0;

		for (var i = 0; i < playerCount; i++)
		{
			var playerNode = (Player)players.GetChild(i);

			Godot.Collections.Array<Variant> player = new()
			{
				Variant.CreateFrom(playerNode.GlobalPosition),
				Variant.CreateFrom(playerNode.ActorRotation)
			};

			dic.Add(Int32.Parse(playerNode.Name), player);
		}

		for (var i = 0; i < playerCount; i++)
		{
			var data = new Dictionary<Variant, Variant>();

			var playerNode = (Player)players.GetChild(i);

			var nearest = playerNode.GetNearestPlayers();

			foreach (var player in nearest.Keys)
			{
				data.Add(player, dic[player.AsInt32()]);
			}

			RpcId(int.Parse(playerNode.Name), "ReceiveWorldState", timestamp, data);
		}
	}

	public void SendActorEnteredZone(int remoteId, Variant actorId, Variant position)
	{
		RpcId(remoteId, "ActorEnteredZone", actorId, position);
	}

	public void SendActorExitedZone(int remoteId, Variant actorId)
	{
		RpcId(remoteId, "ActorExitedZone", actorId);
	}

	public void SendPlayableActor(int remoteId, Variant actorId)
	{
		RpcId(remoteId, "SpawnActorPlayable", actorId);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveWorldState(double timestamp, Variant players) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorEnteredZone(Variant id, Variant position) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorExitedZone(Variant id) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void SpawnActorPlayable(Variant id) { }

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void RequestSkill(Variant id)
	{
		GD.Print("Received Request skill: ", Multiplayer.GetRemoteSenderId());
		var player = players.GetNode<Player>(Multiplayer.GetRemoteSenderId().ToString());

		var nearest = player.GetNearestPlayers();

		RpcId(Multiplayer.GetRemoteSenderId(), "SkillApproved", Multiplayer.GetRemoteSenderId(), id);

		foreach (var p in nearest.Values)
		{
			RpcId(((Player)p).ActorID, "SkillApproved", player.ActorID, id);
		}

		player.RunSkill(id);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SkillApproved(Variant playerId, Variant skillId) { }
}
