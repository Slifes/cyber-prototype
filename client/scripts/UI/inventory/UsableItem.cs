using Godot;
using Packets.Client;

partial class UsableItem : InventoryItem, IUsable
{
  public Node GetData()
  {
    return this;
  }

  public bool IsAvailable()
  {
    return item.Type == ItemType.Active;
  }

  public void Use()
  {
    if (IsAvailable())
    {
      NetworkManager.Instance.SendPacket(new PlayerUseItem
      {
        itemId = item.ID
      });
    }
  }
}
