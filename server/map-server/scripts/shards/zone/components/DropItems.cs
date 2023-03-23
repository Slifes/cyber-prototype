using Godot;
using System.Collections.Generic;

struct Drop
{
  public int ID;
  public int ActorId;
  public int ItemId;
  public ulong TickTime;
}

partial class DropItems : Node
{
  [Export]
  public ulong LifeTime = 10000;

  [Export]
  public ulong OpenToAnyoneTime = 5000;

  Dictionary<int, Drop> droppedItems = new();

  public int AddItem(int actorId, int itemId)
  {
    var dropId = Multiplayer.MultiplayerPeer.GetUniqueId();

    droppedItems.Add(dropId, new Drop
    {
      ID = dropId,
      ActorId = actorId,
      ItemId = itemId,
      TickTime = Time.GetTicksMsec()
    });

    return dropId;
  }

  public void PickUp(ZoneActor actor, int dropId)
  {
    if (droppedItems.ContainsKey(dropId))
    {
      var drop = droppedItems[dropId];

      if (actor.GetActorID() == drop.ActorId || (Time.GetTicksMsec() - drop.TickTime) > OpenToAnyoneTime)
      {
        // Zone.SendDropItemRemove()

        droppedItems.Remove(dropId);
      }
    }
  }

  public override void _Process(double delta)
  {
    var ticks = Time.GetTicksMsec();

    foreach (var item in droppedItems)
    {
      if (ticks - item.Value.TickTime > LifeTime)
      {
        // Zone.SendDropItemRemove()

        droppedItems.Remove(item.Key);
      }
    }
  }
}
