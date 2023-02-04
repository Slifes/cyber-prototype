using Godot;

partial class MovementNetwork : IComponent
{
  bool moveStoppedSended;

  float updateNetworkTime;

  float limitNetworkTime = 1.0f / 20.0f;

  Player actor;

  public MovementNetwork(Player player)
  {
    this.actor = player;
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta)
  {
    updateNetworkTime += (float)delta;

    if (actor.Velocity == Vector3.Zero)
    {
      if (!moveStoppedSended)
      {
        actor.SendMoveStopped();
        moveStoppedSended = true;
      }

    }
    else if (updateNetworkTime > limitNetworkTime)
    {
      actor.SendMoving();

      updateNetworkTime = 0.0f;

      moveStoppedSended = false;
    }
  }
}
