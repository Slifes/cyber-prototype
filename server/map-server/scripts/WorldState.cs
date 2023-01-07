using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
		// UpdatePlayersWithWorldState();
	}

	private void UpdatePlayersWithWorldState()
	{
		Dictionary dic = new();

		var playerCount = players.GetChildCount();

		var timestamp = Time.GetUnixTimeFromSystem() * 1000.0;

		for (var i = 0; i < playerCount; i++)
		{
			var playerNode = (Player)players.GetChild(i);

			if (playerNode.Changed)
			{
				Godot.Collections.Array<Variant> player = new()
				{
					Variant.CreateFrom(playerNode.GlobalPosition),
					Variant.CreateFrom(playerNode.ActorRotation)
				};

				dic.Add(playerNode.ActorID, player);

				playerNode.Changed = false;
			}
		}

		var playersChangedKeys = dic.Keys;

		for (var i = 0; i < playerCount; i++)
		{
			var data = new Godot.Collections.Dictionary<Variant, Variant>();

			var playerNode = (Player)players.GetChild(i);

			var nearest = playerNode.GetNearestPlayers();

			var keys = new List<int>();

			for (var a = 0; a < nearest.Count; a++)
			{
				if (playersChangedKeys.Contains(a))
				{
					keys.Add(a);
				}
			}

			foreach (var player in keys)
			{
				data.Add(player, dic[player]);
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

	public void SendPacketToPlayerNear(Player player, string func, params Variant[] args)
	{
		var nearest = player.GetNearestPlayers();

		foreach (var near in nearest)
		{
			RpcId(near, func, args);
		}
	}

	public void SendServerMovement(Player player, Vector3 position, float yaw)
	{
		SendPacketToPlayerNear(player, "ReceiveMovement", player.ActorID, position, yaw, Now());
	}

	public void SendServerMovementStopped(Player player, Vector3 position, float yaw)
	{
		SendPacketToPlayerNear(player, "ReceiveMovementStopped", player.ActorID, position, yaw, Now());
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

		var now = Variant.CreateFrom(Now());

		RpcId(player.ActorID, "SkillApproved", player.ActorID, id, now);

		foreach (var p in nearest)
		{
			RpcId(p, "SkillApproved", player.ActorID, id, now);
		}

		player.RunSkill(id);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SkillApproved(Variant playerId, Variant skillId, Variant timestamp) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovementStopped(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }
}
