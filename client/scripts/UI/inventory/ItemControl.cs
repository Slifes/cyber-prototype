using System.Collections.Generic;
using System.Linq;

class ItemControl
{
  List<InventoryItem> items;

  private static ItemControl _instance;

  public static ItemControl Instance { get { return _instance; } }

  public static void CreateInstance()
  {
    _instance = new ItemControl();
  }

  ItemControl()
  {
    items = new();
  }

  public void Add(InventoryItem item)
  {
    items.Add(item);
  }

  // public void UpdateItems(int itemId, int amount)
  // {
  //   var items = items.FindAll(x => x.skill.ID == ID).ToList();

  //   items.ForEach(x => x.Used());
  // }

  // public void Remove(SkillItem item)
  // {
  //   items.Remove(item);
  // }
}
