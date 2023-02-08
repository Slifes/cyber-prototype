using Godot;

partial class AreaSkillBase : Area3D
{
  protected IActor actor;

  [Export]
  public float LifeTime;

  private float CurrentTime = 0.0f;

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
