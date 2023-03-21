using Godot;
using Packets.Server;

partial class Zone
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(int peerId, int actorId, int type, Vector3 position, float yaw, Variant data)
  {
    AddActorToNearest(peerId, actorId, (ActorType)type);

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
    RemoveActorFromNearests(peerId, actorId, (ActorType)type);

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
  public void NpcMoving(int actorId, Vector3 position, float yaw)
  {
    Networking.Instance.SendPacketToMany(GetPlayerNearest(actorId), new SMActorStartMove
    {
      ActorId = actorId,
      Position = new float[3] { position.X, position.Y, position.Z },
      Yaw = yaw,
      Tick = Time.GetTicksMsec()
    });
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(int actorId, int skillId, Variant data)
  {
    var actor = spawner.Get((int)actorId);

    if (actor != null)
    {
      actor.EmitSignal(ZoneActor.SignalName.ExecuteSkill, skillId, data);
    }
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ExecuteSkill(int actorId, int actorType, int skillId, Variant data)
  {
    var peers = GetPlayerNearest(actorId);

    Networking.Instance.SendPacketToMany(actorId, peers, new Packets.Server.SMActorExecuteSkill
    {
      SkillId = skillId,
      ActorId = actorId,
      ActorType = actorType
    });
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void UseItem(int actorId, int itemId)
  {
    var item = ItemManager.Instance.Get(itemId);

    GD.Print("Item: ", item);

    if (item != null && item.Type == ItemType.Active)
    {
      var usable = (Usable)item;

      var actor = (PlayerZone)spawner.Get(actorId);

      GD.Print("Send Usable skill");

      actor.EmitSignal(ZoneActor.SignalName.ExecuteSkill, usable.skillId, new Variant());
    }
  }

  [Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEffect(int actorId, int actorType, int effectType, int effectValue)
  {
    if ((ActorType)actorType == ActorType.Player)
    {
      var actor = SessionManager.Instance.GetActor(actorId);

      switch ((EffectType)effectType)
      {
        case EffectType.Damage:
          actor.TakeDamage(effectValue);
          break;
      }
    }

    Networking.Instance.SendPacketToMany(actorId, GetPlayerNearest(actorId), new SMActorEffect
    {
      ActorId = actorId,
      ActorType = actorType,
      EffectType = effectType,
      Value = effectValue
    });
  }
}
