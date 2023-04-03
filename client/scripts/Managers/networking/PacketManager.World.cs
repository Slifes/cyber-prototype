using Godot;
using Packets.Server;

partial class PacketManager
{
  void OnActorEnteredZone(IServerCommand command)
  {
    Spawner.Instance.Spawn((SMActorEnteredZone)command);
  }

  void OnActorExitedZone(IServerCommand command)
  {
    Spawner.Instance.Unspawn((SMActorExitedZone)command);
  }

  void OnActorExecuteSkill(IServerCommand command)
  {
    var pck = (SMActorExecuteSkill)command;

    var actor = Spawner.Instance.GetActor<CharacterActor>(pck.ActorId);

    actor.EmitSignal(CharacterActor.SignalName.ExecuteSkill, pck.SkillId);
  }

  void ActorPushCommand(int actorId, IServerCommand command)
  {
    var actor = Spawner.Instance.GetActor<CharacterActor>(actorId);

    actor.PushCommand(command);
  }

  void OnActorStartMove(IServerCommand command)
  {
    var pck = (SMActorStartMove)command;

    ActorPushCommand(pck.ActorId, command);
  }

  void OnActorStopMove(IServerCommand command)
  {
    var pck = (SMActorStopMove)command;

    ActorPushCommand(pck.ActorId, command);
  }

  void OnActorEffect(IServerCommand command)
  {
    var pck = (SMActorEffect)command;

    var actor = Spawner.Instance.GetActor<CharacterActor>(pck.ActorId);

    actor.EmitSignal(CharacterActor.SignalName.Effect, pck.EffectType, pck.Value);
  }

  void OnActorDrop(IServerCommand command)
  {
    GD.Print("Dropped Item");

    var pck = (SMActorDroppedItems)command;

    GD.Print(pck);

    var actor = Spawner.Instance.GetActor<CharacterActor>(pck.ActorId);

    var packedScene = ResourceLoader.Load<PackedScene>("res://items/icon.tscn");

    foreach (var item in pck.Items)
    {
      GD.Print("Item data: ", item.itemId);

      var itemData = ItemManager.Instance.GetItem(item.itemId);

      var instance = packedScene.Instantiate<ItemDropped>();

      instance.Name = item.dropId.ToString();
      instance.item = itemData;
      instance.Position = actor.GlobalPosition;

      actor.GetNode("/root/World/Items").AddChild(instance);
    }
  }

  void OnItemDroppedRemove(IServerCommand command)
  {
    var pck = (SMDroppedItemRemove)command;

    var node = Spawner.Instance.GetNode("/root/World/Items/" + pck.dropId.ToString());

    if (node != null)
    {
      node.QueueFree();
    }
  }
}
