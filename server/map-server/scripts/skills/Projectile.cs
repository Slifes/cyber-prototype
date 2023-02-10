using Godot;

partial class Projectile : PhysicalAttack
{
  [Export]
  public float Speed;

  public override void _Process(double delta)
  {
    base._Process(delta);

    Position += Direction * (float)delta * Speed;
  }
}
