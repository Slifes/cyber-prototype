using Godot;
using Packets.Server;

partial class Zone
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(int peerId, int actorId, int type, Vector3 position, float yaw, Variant data)
  {
    nearests.Add(peerId, actorId, (ActorType)type);

    Networking.Instance.SendPacket(peerId, new SMActorEnteredZone
    {
      ActorId = actorId,
      ActorType = type,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw,
      Data = data.AsByteArray()
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(int peerId, int actorId, int type)
  {
    nearests.Remove(peerId, actorId, (ActorType)type);

    Networking.Instance.SendPacket(peerId, new SMActorExitedZone
    {
      ActorId = actorId,
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

    SendPacketToAllNearest(actorId, new SMActorStartMove
    {
      ActorId = actorId,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw,
      Tick = Time.GetTicksMsec()
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMovingStop(int actorId, Vector3 position, float yaw)
  {
    SessionActor actor = (SessionActor)PlayerSpawner.Instance.GetNode<Node3D>(actorId.ToString());

    actor.Position = new Vector3(position.X, position.Y, position.Z);
    actor.Rotation = new Vector3(0, yaw, 0);

    SendPacketToAllNearest(actorId, new SMActorStartMove
    {
      ActorId = actorId,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw,
      Tick = Time.GetTicksMsec()
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcMoving(int actorId, Vector3 position, float yaw)
  {
    SendPacketToAllNearest(actorId, new SMActorStartMove
    {
      ActorId = actorId,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw,
      Tick = Time.GetTicksMsec()
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorState(int actorId, int actorType, int state)
  {
    SendPacketToAllNearest(actorId, new SMActorState
    {
      ActorId = actorId,
      ActorType = actorType,
      State = state
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEffect(int actorId, int actorType, int effectType, int effectValue)
  {
    SendPacketToAllNearestAndMe(actorId, new SMActorEffect
    {
      ActorId = actorId,
      ActorType = actorType,
      EffectType = effectType,
      Value = effectValue
    });
  }

  public static void SendActorDead(int actorId, ActorType type)
  {
    Instance.Rpc("ActorDead", actorId, (int)type);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorDead(int actorId, int actorType)
  {
    if (((ActorType)actorType) == ActorType.Npc)
    {
      nearests.RemoveActorList(actorId);
    }
  }
}
