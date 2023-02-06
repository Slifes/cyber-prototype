using Godot;

enum SkillType
{
  Active,
  Passive,
  Buff
}

enum SkillActiveType
{
  Hand,
  Projectile,
}

partial class Skill : Resource
{
  [Export]
  public int ID;

  [Export]
  public string Name;

  [Export]
  public int Level;

  [Export]
  public int Damage;

  [Export]
  public SkillType Type;

  [Export]
  public SkillActiveType ActiveType;

  [Export]
  public int SP;

  [Export]
  public float Delay;

  [Export]
  public PackedScene scene;
}
