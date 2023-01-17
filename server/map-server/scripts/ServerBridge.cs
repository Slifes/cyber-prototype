using Godot;
using Godot.Collections;

partial class ServerBridge : Node3D
{
	CharacterSpawner players;

	public static double Now()
	{
		return Time.GetUnixTimeFromSystem() * 1000.0;
	}

	public override void _Ready()
	{
		players = GetNode<CharacterSpawner>("/root/World/Spawner/players");
	}

	public void SendPacketToPlayerNear(Player player, string func, params Variant[] args)
	{
		var nearest = player.GetNearestPlayers();

		foreach (var near in nearest)
		{
			RpcId(near, func, args);
		}
	}

	#region spawn
	public void SendActorEnteredZone(int remoteId, Actor actor)
	{
		RpcId(remoteId, "ActorEnteredZone", actor.ActorID, (Variant)(int)actor.Type, actor.GlobalPosition, actor.GetData());
	}

	public void SendActorExitedZone(int remoteId, Actor actor)
	{
		RpcId(remoteId, "ActorExitedZone", actor.ActorID, (Variant)(int)actor.Type);
	}

	public void SendPlayableActor(int remoteId, Actor actor)
	{
		RpcId(remoteId, "ActorPlayable", actor.ActorID, actor.GlobalPosition, actor.GetData());
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorEnteredZone(Variant id, Variant type, Variant position, Variant data) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorExitedZone(Variant id, Variant type) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorPlayable(Variant id, Variant position, Variant data) { }

	#endregion

	#region skills
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

	#endregion

	#region PlayerMovement
	public void SendServerMovement(Player player, Vector3 position, float yaw)
	{
		SendPacketToPlayerNear(player, "ReceiveMovement", player.ActorID, position, yaw, Now());
	}
	public void SendServerMovementStopped(Player player, Vector3 position, float yaw)
	{
		SendPacketToPlayerNear(player, "ReceiveMovementStopped", player.ActorID, position, yaw, Now());
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovementStopped(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }

	#endregion
}
