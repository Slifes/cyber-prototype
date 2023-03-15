using Godot;
using System.Collections.Generic;
using Packets.Server;

class Inventory
{
  Player player;

  struct ItemData
  {
    public Item item;
    public int amount;
  }

  Dictionary<int, ItemData> items;

  public Inventory(Player player)
  {
    items = new();

    this.player = player;
  }

  public void Remove(int itemId, int amount)
  {
    if (!items.ContainsKey(itemId))
    {
      return;
    }

    var item = items[itemId];

    if (amount >= item.amount)
    {
      items.Remove(itemId);
    }
    else
    {
      item.amount -= amount;
    }

    Networking.Instance.SendPacket(player.GetActorId(), new SMInventoryRemoveItem
    {
      itemId = itemId,
      amount = item.amount
    });
  }

  public void Add(int itemId, int amount)
  {
    var item = ItemManager.Instance.Get(itemId);

    if (item == null) return;

    if (items.ContainsKey(itemId))
    {
      var data = items[itemId];

      data.amount += amount;
    }
    else
    {
      items[itemId] = new ItemData
      {
        item = item,
        amount = amount
      };
    }

    Networking.Instance.SendPacket(player.GetActorId(), new SMInventoryAddItem
    {
      itemId = itemId,
      amount = amount
    });
  }
}
