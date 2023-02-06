using Godot;

using System.Collections.Generic;

partial class ChatSystem : Control
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void LocalMessage(List<int> peers, Variant actorId, Variant message)
  {
    RpcId(0, "ReceiveLocalMessage", actorId, message);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ReceiveLocalMessage(Variant actorId, Variant message) { }
}
