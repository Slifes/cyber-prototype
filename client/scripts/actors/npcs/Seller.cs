using Godot;
using Godot.Collections;

partial class Seller: BaseNPC
{
  [Export]
  public Array<Item> items;

  Array<Node> actorsTalking;

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)] 
  void ListItems(Array<int> item, Array<float> price) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActionBuy(Variant item, Variant amount) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActionSell(Variant item, Variant amount) { }
}