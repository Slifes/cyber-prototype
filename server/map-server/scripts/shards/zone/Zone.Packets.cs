using Godot;
using Packets.Server;

partial class Zone
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(int peerId, int actorId, int type, Vector3 position, float yaw, Variant data, bool broadcast)
  {
    AddActorToNearest(peerId, actorId, (ActorType)type);

    if (!broadcast) return;

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
  public void ActorExitedZone(int peerId, int actorId, int type, bool broadcast)
  {
    RemoveActorFromNearests(peerId, actorId, (ActorType)type);

    if (!broadcast) return;

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

    Networking.Instance.SendPacketToMany(GetPlayerNearest(actorId), new SMActorStartMove
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

    Networking.Instance.SendPacketToMany(GetPlayerNearest(actorId), new SMActorStartMove
    {
      ActorId = actorId,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw,
      Tick = Time.GetTicksMsec()
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcUpdateState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

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

    Networking.Instance.SendPacketToMany(actorId, peers, new Packets.Server.SMExecuteSkill
    {
      SkillId = skillId,
      ActorId = actorId,
      ActorType = (int)ActorType.Player
    });
  }

  [Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorTakeDamage(int actorId, int actorType, int damage)
  {
    Networking.Instance.SendPacketToMany(actorId, GetPlayerNearest(actorId), new SMActorDamage
    {
      ActorId = actorId,
      ActorType = actorType,
      Damage = damage
    });
  }
}
