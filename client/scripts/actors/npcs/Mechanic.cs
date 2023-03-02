using Godot;

partial class Mechanic : Talk
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void AttachEquipment(Variant item) { }
}
