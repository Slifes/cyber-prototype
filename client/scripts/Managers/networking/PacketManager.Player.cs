using Packets.Server;

partial class PacketManager
{
  void OnInventoryAddItem(IServerCommand command)
  {
    var pck = (SMInventoryAddItem)command;

    PlayerUI.Instance.inventory.Add(pck.itemId, pck.amount);
  }

  void OnInventoryRemoveItem(IServerCommand command)
  {
    var pck = (SMInventoryRemoveItem)command;

    PlayerUI.Instance.inventory.Remove(pck.itemId, pck.amount);
  }
}
