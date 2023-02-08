using Godot;

partial class Projectile : PhysicalAttack
{
  [Export]
  public float Speed;

  public Vector3 Direction = Vector3.Forward;

  public override void _Process(double delta)
  {
    base._Process(delta);

    GlobalPosition += Direction * (float)delta * Speed;
  }
}
