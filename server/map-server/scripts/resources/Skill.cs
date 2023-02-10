using Godot;

partial class Skill : Base
{
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
