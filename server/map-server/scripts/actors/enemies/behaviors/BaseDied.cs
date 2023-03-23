using Godot;

class BaseDied : IBehavior
{
  Behavior behavior;

  float timeDied = 0;

  public BaseDied(Behavior actor)
  {
    this.behavior = actor;
  }

  public void Finish()
  {
    behavior.Actor.QueueFree();
  }

  public void Handler(double delta)
  {
    timeDied += (float)delta;

    if (timeDied > 5.0f)
    {
      Finish();
    }
  }

  public void Start()
  {
    GD.Print("Dead");

    behavior.Actor.SendDead();
  }

  public Variant GetData()
  {
    return Vector3.Zero;
  }
}
