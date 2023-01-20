using Godot;
using System.Collections.Generic;

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

  public void SendPacketTo(System.Collections.Generic.List<int> peers, string func, params Variant[] args)
	{
		foreach(var peerId in peers)
		{
			RpcId(peerId, func, args);
		}
	}

	#region spawn
	public void SendActorEnteredZone(int remoteId, IActor actor)
	{
		RpcId(remoteId, "ActorEnteredZone", actor.GetActorId(), (Variant)(int)actor.GetActorType(), ((Node3D)actor).GlobalPosition, actor.GetData());
	}

	public void SendActorExitedZone(int remoteId, IActor actor)
	{
		RpcId(remoteId, "ActorExitedZone", actor.GetActorId(), (Variant)(int)actor.GetActorType());
	}

	public void SendPlayableActor(int remoteId, IActor actor)
	{
		RpcId(remoteId, "ActorPlayable", actor.GetActorId(), ((Node3D)actor).GlobalPosition, actor.GetData());
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorEnteredZone(Variant id, Variant type, Variant position, Variant data) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorExitedZone(Variant id, Variant type) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorPlayable(Variant id, Variant position, Variant data) { }

	#endregion

	#region skills
  public void SendSkillExecutedTo(List<int> peers, IActor actor, int skillId)
  {
	var now = Now();

  if (actor.GetActorType() == ActorType.Player){
	  RpcId(actor.GetActorId(), "SkillExecuted", actor.GetActorId(), (int)actor.GetActorType(), skillId, now);
  }

	SendPacketTo(peers, "SkillExecuted", actor.GetActorId(), (int)actor.GetActorType(), skillId, now);
  }

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void RequestSkill(Variant id)
	{
		GD.Print("Received Request skill: ", Multiplayer.GetRemoteSenderId());
  
		var player = players.GetNode<Player>(Multiplayer.GetRemoteSenderId().ToString());

		SendSkillExecutedTo(player.GetNearestPlayers(), player, (int)id);
  
		player.RunSkill(id);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SkillExecuted(Variant actorId, Variant actorType, Variant skillId, Variant timestamp) { }
	#endregion

	#region PlayerMovement
	public void SendServerMovement(Player player, Vector3 position, float yaw)
	{
		SendPacketTo(player.GetNearestPlayers(), "ReceiveMovement", player.GetActorId(), position, yaw, Now());
	}

	public void SendServerMovementStopped(Player player, Vector3 position, float yaw)
	{
		SendPacketTo(player.GetNearestPlayers(), "ReceiveMovementStopped", player.GetActorId(), position, yaw, Now());
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovementStopped(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }

	#endregion

	#region npc
	public void SendNpcChangeState(System.Collections.Generic.List<int> players, Variant id, Variant state, Variant position, Variant yaw, Variant data)
	{
		SendPacketTo(players, "NpcChangeState", id, state, position, yaw, data, Now());
	}

	public void SendNpcUpdateState(System.Collections.Generic.List<int> players, Variant id, Variant state, Variant position, Variant yaw, Variant data)
	{
		SendPacketTo(players, "NpcUpdateState", id, state, position, yaw, data, Now());
	}

	public void SendNpcAction(System.Collections.Generic.List<int> players, Variant id, Variant action, Variant position, Variant yaw, Variant data)
	{
		SendPacketTo(players, "NpcAction", id, action, position, yaw, data, Now());
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void NpcUpdateState(Variant id, Variant state,Variant position, Variant yaw, Variant data, Variant timestamp) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void NpcAction(Variant id, Variant action, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  public void SendActorTookDamage(IActor actor, int damage)
  {
	  RpcId(actor.GetActorId(), "ActorTookDamage", actor.GetActorId(), damage, actor.GetCurrentHP(), actor.GetMaxHP());
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorTookDamage(Variant actorId, Variant damage, Variant hp, Variant maxHP) { }
	#endregion
}
