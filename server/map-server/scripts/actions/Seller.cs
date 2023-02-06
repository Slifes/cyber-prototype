using Godot;

partial class Seller : Node
{
  public void Buy(Player player, Variant itemId)
  {
    if (player.Zeny.HasEnough(10))
    {
      player.Zeny.Transfer(10);
      player.Inv.AddItem(itemId, 1);
    }
  }

  public void Sell(Player player, Variant itemId, Variant amount)
  {
    player.Zeny.Receive(10);
    player.Inv.RemoveItem(itemId, 1);
  }
}
