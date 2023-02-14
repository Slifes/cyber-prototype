using Godot;
using System.Collections.Generic;

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

    ServerBridge.Instance.SendActorEnteredZone(actorId, id, type, position, yaw, data);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(int actorId, int id, int type)
  {
    if (neraests.ContainsKey(actorId))
    {
      neraests[actorId].Remove(id);
    }

    ServerBridge.Instance.SendActorExitedZone(actorId, id, type);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMoved(int actorId, Vector2 position, float yaw)
  {
    Node3D actor = spawner.Get((int)actorId);

    if (actor == null) { return; }

    actor.Position = new Vector3(position.X, actor.Position.Y, position.Y);
    actor.Rotation = new Vector3(0, yaw, 0);

    Rpc("ActorMoving", actorId, position, yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMoving(int actorId, Vector2 position, float yaw)
  {
    SessionActor actor = (SessionActor)CharacterSpawner.Instance.GetNode<Node3D>(actorId.ToString());

    actor.Position = new Vector3(position.X, actor.Position.Y, position.Y);
    actor.Rotation = new Vector3(0, yaw, 0);

    if (neraests.ContainsKey(actorId))
    {
      ServerBridge.Instance.SendServerMovement(neraests[actorId], actor, actor.Position, yaw);
    }
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
  }
}
