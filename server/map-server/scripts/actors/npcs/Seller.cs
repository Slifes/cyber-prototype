using Godot;
using Godot.Collections;

partial class Seller : BaseNPC
{
  [Export]
  public Array<Item> items;

  Array<Player> actorsTalking;

  void ActionBuy(int peerId, int itemId, int amount)
  {
    var player = PlayerSpawner.Instance.GetNode<Player>(peerId.ToString());

    var item = ItemManager.Instance.Get(itemId);

    if (item != null)
    {
      // player.Zeny.Transfer(amount, this);
      player.Inv.Add(itemId, amount);
    }
  }

  void ActionSell(Variant item, Variant amount)
  {

  }
}
