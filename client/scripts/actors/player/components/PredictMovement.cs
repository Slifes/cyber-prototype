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
    if (LastUpdateTime == 0)
    {
      LastUpdateTime = Time.GetTicksMsec();
      LastPositionUpdated = newPosition;
      return;
    }

    var elapsedTime = (float)(Time.GetTicksMsec() - LastUpdateTime);

    PredictedVelocity = (newPosition - LastPositionUpdated) / elapsedTime;

    LastPositionUpdated = newPosition;
    LastUpdateTime = Time.GetTicksMsec();
  }

  void InterpolatePosition()
  {
    var elapsedTime = (float)(Time.GetTicksMsec() - LastUpdateTime);

    var predictedPosition = LastPositionUpdated + PredictedVelocity * elapsedTime;

    actor.GlobalPosition = predictedPosition;
  }

  public void Update(float delta)
  {
    if (actor.State == PlayerState.Walking)
    {
      InterpolatePosition();
    }
  }
}
