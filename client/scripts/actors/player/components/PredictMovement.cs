using Godot;

class PredictMovement : IComponent
{
  float PredictionDecayFactor = 0.9f;

  double LastUpdateTime = ServerBridge.Now();

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
  }

  public void StartMovement(Variant position, Variant yaw)
  {
    UpdatePosition((Vector3)position);

    actor.SetBodyRotation(new Vector3(0, (float)yaw, 0));
    actor.ChangeState(PlayerState.Walking);
  }

  public void UpdatePosition(Vector3 newPosition)
  {
    var now = ServerBridge.Now();

    // if (LastUpdateTime == 0)
    // {
    //   now = 1;
    // }

    var velocity = (newPosition - actor.GlobalPosition) / (float)(now - LastUpdateTime);

    if (velocity.IsFinite())
    {
      PredictedVelocity = velocity;
    }

    GD.Print("PredictedVelocity", PredictedVelocity);

    actor.GlobalPosition = newPosition;

    LastPositionUpdated = newPosition;

    LastUpdateTime = now;
  }

  void InterpolatePosition()
  {
    var elapsedTime = (float)(ServerBridge.Now() - LastUpdateTime);

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
