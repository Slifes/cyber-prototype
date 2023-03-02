using Godot;
using System.Collections.Generic;
using Packets.Server;

partial class Zone
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(int actorId, int id, int type, Vector3 position, float yaw, Variant data)
  {
    if (!neraests.ContainsKey(actorId))
    {
      neraests.Add(actorId, new List<int>() { id });
    }
    else
    {
      neraests[actorId].Add(id);
    }

    Networking.Instance.SendPacket(actorId, new SMActorEnteredZone
    {
      ActorId = id,
      ActorType = type,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(int actorId, int id, int type)
  {
    if (neraests.ContainsKey(actorId))
    {
      neraests[actorId].Remove(id);
    }

    Networking.Instance.SendPacket(actorId, new SMActorExitedZone
    {
      ActorId = id,
      ActorType = type
    });
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorStartMove(int actorId, Vector3 position, float yaw)
  {
    Node3D actor = spawner.Get((int)actorId);

    if (actor == null) { return; }

    actor.Position = new Vector3(position.X, position.Y, position.Z);
    actor.Rotation = new Vector3(0, yaw, 0);

    Rpc("ActorMoving", actorId, position, yaw);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorStopMove(int actorId, Vector3 position, float yaw)
  {
    Node3D actor = spawner.Get((int)actorId);

    if (actor == null) { return; }

    actor.Position = new Vector3(position.X, position.Y, position.Z);
    actor.Rotation = new Vector3(0, yaw, 0);

    Rpc("ActorMovingStop", actorId, position, yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMoving(int actorId, Vector3 position, float yaw)
  {
    SessionActor actor = (SessionActor)PlayerSpawner.Instance.GetNode<Node3D>(actorId.ToString());

    actor.Position = new Vector3(position.X, position.Y, position.Z);
    actor.Rotation = new Vector3(0, yaw, 0);

    // ServerBridge.Instance.SendServerMovement(GetPlayerNearest(actorId), actor, actor.Position, yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMovingStop(int actorId, Vector3 position, float yaw)
  {
    SessionActor actor = (SessionActor)PlayerSpawner.Instance.GetNode<Node3D>(actorId.ToString());

    actor.Position = new Vector3(position.X, position.Y, position.Z);
    actor.Rotation = new Vector3(0, yaw, 0);

    // ServerBridge.Instance.SendServerMovementStopped(GetPlayerNearest(actorId), actor, actor.Position, yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcUpdateState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorTookDamage(Variant actorId, Variant actorType, Variant damage, Variant hp, Variant maxHP) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant actorId, Variant skillId, Variant data)
  {
    var actor = spawner.Get((int)actorId);

    if (actor != null)
    {
      actor.EmitSignal(ZoneActor.SignalName.ExecuteSkill, skillId, data);
    }

    Rpc("ExecuteSkill", actorId, skillId, data);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ExecuteSkill(int actorId, int skillId, Variant data)
  {
    var peers = GetPlayerNearest(actorId);

    //ServerBridge.Instance.SendSkillExecutedTo(peers, actor, skillId);

    Networking.Instance.SendPacketToMany(peers, new Packets.Server.SMExecuteSkill
    {
      SkillId = skillId,
      ActorId = actorId,
      ActorType = (int)ActorType.Player
    });
  }
}
