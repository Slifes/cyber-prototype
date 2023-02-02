using Godot;

partial class PredictMovement : IPlayerComponent
{
  float PredictionDecayFactor = 0.9f;

  double LastUpdateTime = ServerBridge.Now();

  Vector3 PredictedVelocity = Vector3.Zero;

  Vector3 LastPositionUpdated = Vector3.Zero;

  Player actor;

  public PredictMovement(Player player)
  {
    actor = player;
  }

  public void InputHandler(InputEvent @event) { }

  public void UpdatePosition(Vector3 newPosition)
  {
    var now = ServerBridge.Now();

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
