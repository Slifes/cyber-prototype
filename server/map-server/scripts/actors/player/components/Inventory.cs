using Godot;
using System.Collections.Generic;

class Inventory : IComponent
{
  List<Item> items;

  public Inventory(IActor actor)
  {
    items = new();
  }

  public void RemoveItem(Variant itemId, Variant amount)
  {

  }

  public void AddItem(Variant itemId, Variant amount)
  {

  }
}
