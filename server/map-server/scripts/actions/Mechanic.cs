using Godot;

partial class Mechanic : Node
{
  public void AttachToPlayer(Player player, Variant equipId)
  {
    if (player.Zeny.HasEnough(10))
    {
      player.Zeny.Transfer(10);
      player.Equip.ApplyEquipment(equipId);
    }
  }

  // [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  // public void OnAttachEquipmenetOnPlayer(int equipId)
  // {
  //   Multiplayer.GetRemoteSenderId();
  // }
}
