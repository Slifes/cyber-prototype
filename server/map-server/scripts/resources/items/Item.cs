using Godot;

partial class Item : Base
{
  [Export]
  public string Name;

  [Export]
  public ItemType Type;

  [Export]
  public int Price;
}
