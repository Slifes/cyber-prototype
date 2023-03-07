using Godot;

partial class PhysicalAttack : AreaSkillBase
{
  [Export]
  public int MinDamage;

  [Export]
  public int MaxDamage;

  public override void _Ready()
  {
    base._Ready();

    BodyEntered += Collided;
  }

  int CalculateDamage()
  {
    var damage = GD.RandRange(MinDamage, MaxDamage);

    return Mathf.Abs(damage);
  }

  void Collided(Node body)
  {
    if (body.IsInGroup("Actor"))
    {
      ZoneActor actor = (ZoneActor)body;

      actor.TakeDamage(CalculateDamage());
    }
  }
}
