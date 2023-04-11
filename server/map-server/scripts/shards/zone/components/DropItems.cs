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
  public static DropItems Instance { get; set; }

  [Export]
  public ulong LifeTime = 10000;

  [Export]
  public ulong OpenToAnyoneTime = 5000;

  Dictionary<int, Drop> droppedItems = new();

  public override void _Ready()
  {
    Instance = this;
  }

  public void PublishDrop(int actorId, ActorType actorType, int actorTarget, Godot.Collections.Array<int> itemIds)
  {
    var items = new Godot.Collections.Array();

    foreach (var itemId in itemIds)
    {
      var dropId = AddItem(actorTarget, itemId);

      var item = new Godot.Collections.Dictionary<string, int>()
      {
        {"itemId", itemId},
        {"dropId", dropId}
      };

      items.Add(item);
    }

    Zone.SendActorDrop(actorId, actorType, actorTarget, items);
  }

  int AddItem(int actorId, int itemId)
  {
    var dropId = (int)Multiplayer.MultiplayerPeer.GenerateUniqueId();

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
        Zone.SendDropCollected(actor.GetActorID(), dropId, drop.ItemId);

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
        // Zone.SendDropItemRemove(item.Key);

        droppedItems.Remove(item.Key);
      }
    }
  }
}
