using Godot;
using System.Collections.Generic;

partial class ServerBridge : Node3D
{
  CharacterSpawner players;

  private static ServerBridge _instance;

  public static ServerBridge Instance
  {
    get { return _instance; }
  }

  public static double Now()
  {
    return Time.GetUnixTimeFromSystem() * 1000.0;
  }

  public override void _Ready()
  {
    players = GetNode<CharacterSpawner>("/root/World/Spawner/players");

    _instance = this;
  }

  public void SendPacketTo(System.Collections.Generic.List<int> peers, string func, params Variant[] args)
  {
    foreach (var peerId in peers)
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

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(Variant id, Variant type, Variant position, Variant data) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(Variant id, Variant type) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorPlayable(Variant id, Variant position, Variant data) { }

  #endregion

  #region skills
  public void SendSkillExecutedTo(List<int> peers, IActor actor, int skillId)
  {
    var now = Now();

    if (actor.GetActorType() == ActorType.Player)
    {
      RpcId(actor.GetActorId(), "SkillExecuted", actor.GetActorId(), (int)actor.GetActorType(), skillId, now);
    }

    SendPacketTo(peers, "SkillExecuted", actor.GetActorId(), (int)actor.GetActorType(), skillId, now);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SkillExecuted(Variant actorId, Variant actorType, Variant skillId, Variant timestamp) { }
  #endregion

  #region PlayerDetails

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void PlayerSkillList(Variant actorId, Variant skillIds) { }
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

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
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

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcUpdateState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  public void SendActorTookDamage(List<int> peers, IActor actor, int damage)
  {
    if (actor.GetActorType() == ActorType.Player)
    {
      RpcId(actor.GetActorId(), "ActorTookDamage", actor.GetActorId(), (int)actor.GetActorType(), damage, actor.GetCurrentHP(), actor.GetMaxHP());
    }

    SendPacketTo(peers, "ActorTookDamage", actor.GetActorId(), (int)actor.GetActorType(), damage, actor.GetCurrentHP(), actor.GetMaxHP());
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorTookDamage(Variant actorId, Variant actorType, Variant damage, Variant hp, Variant maxHP) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void UpdateCharacterStatus(Variant actorId, Variant actorType, Variant currentHP, Variant currentSP) { }
  #endregion
}
