using Godot;

class PredictMovement : IComponent
{
  float PredictionDecayFactor = 0.9f;

  double LastUpdateTime = 0;//Time.GetTicksMsec();

  Vector3 PredictedVelocity = Vector3.Zero;

  Vector3 LastPositionUpdated = Vector3.Zero;

  Player actor;

  public PredictMovement(Player player)
  {
    actor = player;

    actor.SvStartMovement += StartMovement;
    actor.SvStopMovement += StopMovement;
  }

  public void InputHandler(InputEvent @event) { }

  public void StopMovement(Variant position, Variant yaw)
  {
    // PredictedVelocity = Vector3.Zero;
    // LastUpdateTime = 0;
    // LastPositionUpdated = (Vector3)position;
    actor.GlobalPosition = (Vector3)position;

    actor.SetBodyRotation(new Vector3(0, (float)yaw, 0));
    actor.ChangeState(PlayerState.Idle);

    LastUpdateTime = 0;
  }

  public void StartMovement(Variant position, Variant yaw)
  {
    UpdatePosition((Vector3)position);

    actor.SetBodyRotation(new Vector3(0, (float)yaw, 0));
    actor.ChangeState(PlayerState.Walking);
  }

  public void UpdatePosition(Vector3 newPosition)
  {
    var now = Time.GetTicksMsec();

    if (LastUpdateTime == 0)
    {
      LastUpdateTime = now - 16;
    }

    var velocity = (newPosition - actor.GlobalPosition) / (float)(now - LastUpdateTime);

    if (velocity.IsFinite())
    {
      PredictedVelocity = velocity;
    }
    else
    {
      GD.Print("NEwPosition: ", newPosition);
      GD.Print("Current Position: ", actor.GlobalPosition);
      GD.Print("Now: ", now);
      GD.Print("LastTime: ", LastUpdateTime);
    }

    GD.Print("PredictedVelocity", PredictedVelocity);

    actor.GlobalPosition = newPosition;

    LastPositionUpdated = newPosition;

    LastUpdateTime = now;
  }

  void InterpolatePosition()
  {
    var elapsedTime = (float)(Time.GetTicksMsec() - LastUpdateTime);

    actor.GlobalPosition = ((LastPositionUpdated + (PredictedVelocity * elapsedTime)) * PredictionDecayFactor) + (LastPositionUpdated * (1 - PredictionDecayFactor));
  }

  public void Update(float delta)
  {
    if (actor.State == PlayerState.Walking)
    {
      InterpolatePosition();
    }
  }
}
