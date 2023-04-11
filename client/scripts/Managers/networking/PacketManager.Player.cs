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

  void OnActorVoice(IServerCommand command)
  {
    var pck = (SMActorVoiceData)command;

    var player = Spawner.Instance.GetActor<Player>(pck.ActorId);

    player.EmitSignal(Player.SignalName.VoiceReceived, pck.Data);
  }
}
