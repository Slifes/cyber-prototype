using Godot;

partial class Dash : Node
{
  ZoneActor actor;

  float Speed;

  Vector3 Direction;

  public override void _PhysicsProcess(double delta)
  {
    base._PhysicsProcess(delta);
  }
}
