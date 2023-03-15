using Godot;

partial class Skill : Base
{
  [Export]
  public string Name;

  [Export]
  public int Level;

  [Export]
  public int MinValue;

  [Export]
  public int MaxValue;

  [Export]
  public SkillType Type;

  [Export]
  public int SP;

  [Export]
  public float Delay;

  [Export]
  public PackedScene Scene;
}
