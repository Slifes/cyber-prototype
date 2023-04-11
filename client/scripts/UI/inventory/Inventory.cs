using Godot;
using System.Collections.Generic;
using System.Linq;

partial class Inventory : Window
{
  [Signal]
  public delegate void ReceiveListEventHandler(Godot.Collections.Array<GodotObject> items);

  static PackedScene ItemScene = ResourceLoader.Load<PackedScene>("res://ui/inventory/item_draggable.tscn");

  HFlowContainer container;

  List<UsableItem> items;

  public override void _Ready()
  {
    container = GetNode<HFlowContainer>("TabContainer/Consume/ScrollContainer/VBoxContainer/HFlowContainer");

    items = new();
  }

  public void Add(int itemId, int amount)
  {
    if (!ContainsSpace(itemId))
    {
      Control item = AddItemControl(itemId, amount);

      container.AddChild(item);
    }
    else
    {
      var itemsUpdate = items.Where((x) => x.item.ID == itemId).ToList();

      var amountToBeUpdated = amount;

      foreach (var item in itemsUpdate)
      {
        item.Amount = item.Amount + amount;
        break;
      }
    }
  }

  public void Remove(int itemId, int amount)
  {
    var queryset = items.Where((x) => x.item.ID == itemId);

    var amountToBeRemoved = amount;

    foreach (var item in queryset.ToList())
    {
      if (item.Amount <= amountToBeRemoved)
      {
        item.GetParent().QueueFree();

        items.Remove(item);

        amountToBeRemoved -= item.Amount;

        if (amountToBeRemoved <= 0) { break; }
      }
      else
      {
        item.Amount = item.Amount - amountToBeRemoved;
        break;
      }
    }
  }

  bool ContainsSpace(int itemId)
  {
    return items.Where((x) => x.item.ID == itemId).Count() > 0;
  }

  Control AddItemControl(int itemId, int amount)
  {
    var instance = ItemScene.Instantiate<UsableItem>();

    instance.item = ItemManager.Instance.GetItem(itemId);
    instance.Amount = amount;

    items.Add(instance);

    return DragHelper.Create(instance, new Vector2(40, 40));
  }
}
