using Godot;

partial class Item : Resource
{
  [Export]
  public int ID;

  [Export]
  public string Name;

  [Export]
  public string Description;

  [Export]
  public float Price;

  [Export]
  public int SkillID;
}
