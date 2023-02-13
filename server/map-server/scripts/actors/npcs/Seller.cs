using Godot;
using Godot.Collections;

partial class Seller : BaseNPC
{
  [Export]
  public Array<Item> items;

  Array<Node> actorsTalking;

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ListItems(Array<int> item, Array<float> price) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActionBuy(Variant item, Variant _amount)
  {
    var itemId = (int)item;
    var amount = (int)_amount;

    var peerId = Multiplayer.GetRemoteSenderId();

    var player = CharacterSpawner.Instance.GetNode<Player>(peerId.ToString());
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActionSell(Variant item, Variant amount) { }
}
