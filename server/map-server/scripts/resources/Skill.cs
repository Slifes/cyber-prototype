using Godot;

partial class Skill : Resource
{
  [Export]
  public int ID;

  [Export]
  public string Name;

  [Export]
  public int Level;

  [Export]
  public int MinDamage;

  [Export]
  public int MaxDamage;

  [Export]
  public SkillType Type;

  [Export]
  public int SP;

  [Export]
  public float Delay;

  [Export]
  public PackedScene Scene;
}
