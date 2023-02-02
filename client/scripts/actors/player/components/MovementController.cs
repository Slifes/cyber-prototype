using Godot;
using System;

class MovementController : IPlayerComponent
{
  Player actor;

  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

  public MovementController(Player player)
  {
    this.actor = player;
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta)
  {
    Vector3 velocity = actor.Velocity;

    // Add the gravity.
    if (!actor.IsOnFloor())
      velocity.Y -= gravity * (float)delta;

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    Vector2 inputDir = Input.GetVector("left", "right", "up", "down");

    if (inputDir != Vector2.Zero)
    {
      Vector3 direction = (actor.Body.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

      velocity.X = direction.X * Player.Speed;
      velocity.Z = direction.Z * Player.Speed;

      moveStoppedSended = false;
    }
    else
    {
      velocity.X = Mathf.MoveToward(velocity.X, 0, Player.Speed);
      velocity.Z = Mathf.MoveToward(velocity.Z, 0, Player.Speed);
    }

    actor.Velocity = velocity;

    actor.MoveAndSlide();
  }
}
