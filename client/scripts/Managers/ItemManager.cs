using Godot;
using System.Collections.Generic;

class ItemManager
{
  Dictionary<int, Item> items;

  private static ItemManager _instance;

  public static ItemManager Instance { get { return _instance; } }

  public static void CreateInstance()
  {
    _instance = new ItemManager();
  }

  public ItemManager()
  {
    items = new();
  }

  public Item GetItem(int id)
  {
    if (items.ContainsKey(id))
    {
      return items[id];
    }

    var item = ResourceLoader.Load<Item>(string.Format("res://resources/items/{0}.tres", id));

    items.Add(id, item);

    return item;
  }
}
