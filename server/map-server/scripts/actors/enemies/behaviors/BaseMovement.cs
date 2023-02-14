using Godot;

class BaseMovement : IBehavior
{
  Behavior behavior;

  Area3D AgressiveArea;

  public BaseMovement(Behavior actor)
  {
    behavior = actor;

    AgressiveArea = actor.Actor.GetNode<Area3D>("AgressiveArea");
  }

  public void Start()
  {
    AgressiveArea.BodyEntered += TargetEntered;
  }

  private void TargetEntered(Node3D body)
  {
    if (body.IsInGroup("Actor"))
    {
      behavior.Target = (ZoneActor)body;
      behavior.ChangeState(AIState.Steering);
    }
  }

  public void Handler(double delta)
  {
    if (!behavior.Actor.IsOnFloor())
    {
      GD.Print("Not floor");
      behavior.Actor.Velocity -= new Vector3(0, 9.8f * (float)delta, 0);

      behavior.Actor.MoveAndSlide();
    }
  }

  public void Finish()
  {
    AgressiveArea.BodyEntered -= TargetEntered;
  }

  public Variant GetData()
  {
    return Vector3.Zero;
  }
}
