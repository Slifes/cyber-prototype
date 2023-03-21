using Godot;


enum ItemType
{
  Active,
  Piece,
  Equipment,
}

partial class Item : Resource
{
  [Export]
  public int ID;

  [Export]
  public ItemType Type;

  [Export]
  public string Name;

  [Export]
  public string Description;

  [Export]
  public Color Icon;

  [Export]
  public PackedScene Mesh;

  [Export]
  public int StackLimit;
}
