using Godot;

class BasicMovement
{
  Vector3 targetLocation;

  bool FixY(CharacterBody3D actor, float delta)
  {
    if (!actor.IsOnFloor())
    {
      Vector3 velocity = actor.Velocity;

      velocity.y -= 9.8f * delta;

      actor.Velocity = velocity;

      actor.MoveAndSlide();

      return false;
    }

    return true;
  }

  private void Handle(CharacterBody3D actor, double delta)
  {
    /*if (targetLocation == Vector3.Zero)
    {
      var random = new RandomNumberGenerator();

      var mobX = random.Randf() * 4;
      var mobY = random.Randf() * 4;

      GD.Print("New position: ", mobX, mobY);

      targetLocation = new Vector3(mobX, 0, mobY);

      agent.TargetLocation = targetLocation;
    }

    if (agent.IsTargetReachable())
    {
      if (agent.IsTargetReached())
      {
        state = MobState.Idle;
        targetLocation = Vector3.Zero;
      }
      else
      {
        var next = agent.GetNextLocation();
        var velocity = GlobalPosition.DirectionTo(next).Normalized() * Speed * (float)delta;

        actor.Velocity = velocity;

        SendToAllActors("MoveToTarget", GlobalPosition, velocity);

        actor.MoveAndSlide();
      }
    }*/
  }
}