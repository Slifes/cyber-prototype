using Godot;

partial class AreaSkillBase : Area3D
{
  protected IActor actor;

  [Export]
  public float LifeTime;

  protected Vector3 Direction = Vector3.Forward;

  private float CurrentTime = 0.0f;

  public override void _Ready()
  {
    base._Ready();

    Direction = Vector3.Forward.Rotated(Vector3.Up, Rotation.Y);
  }

  protected void MaybeRemoveByLifetime(double delta)
  {
    CurrentTime += (float)delta;

    if (CurrentTime >= LifeTime)
    {
      QueueFree();
    }
  }
  public override void _Process(double delta)
  {
    MaybeRemoveByLifetime(delta);

    base._Process(delta);
  }
}
