using Godot;
using System;

partial class Player
{
  [Export]
  float MouseWheelVelocity = .5f;

  [Export]
  float MouseWheelUpLimit = 1.0f;

  [Export]
  float MouseWheelDownLimit = 10.0f;

  float time = 0.0f;
    
  bool moveStoppedSended = false;

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton)
    {
      InputEventMouseButton emb = (InputEventMouseButton)@event;
      if (emb.IsPressed())
      {
        if (emb.ButtonIndex == MouseButton.WheelUp && camera3d.Size > MouseWheelUpLimit)
        {
          //camera3d.Size -= MouseWheelVelocity;
          camera3d.Position -= new Vector3(0, MouseWheelVelocity, 0.20f);
          GD.Print("Camera Size: ", camera3d.Size);
        }
        if (emb.ButtonIndex == MouseButton.WheelDown && camera3d.Size < MouseWheelDownLimit)
        {
          camera3d.Position += new Vector3(0, MouseWheelVelocity, 0.20f);
          //camera3d.Size += MouseWheelVelocity;
        }
      }
    }
  }

  void _RotateCamera(double delta)
  {
    bool isPressed = Input.IsMouseButtonPressed(MouseButton.Right);

    if (isPressed)
    {
      if (mouseCameraPressed)
      {
        Vector2 currentMousePosition = GetViewport().GetMousePosition();

        float x = mouseMoveCameraInitial.X - currentMousePosition.X;

        float velocity = 0.2f * x * (float)delta;

        camera.RotateY(velocity);

        mouseMoveCameraInitial = currentMousePosition;
      }
      else
      {
        mouseCameraPressed = true;
        mouseMoveCameraInitial = GetViewport().GetMousePosition();
      }
    }
    else
    {
      mouseCameraPressed = false;
    }
  }

  void _MoveCharacter(double delta)
  {
    Vector3 velocity = Velocity;

    // Add the gravity.
    if (!IsOnFloor())
      velocity.Y -= gravity * (float)delta;

    // Get the input direction and handle the movement/deceleration.
    // As good practice, you should replace UI actions with custom gameplay actions.
    Vector2 inputDir = Input.GetVector("left", "right", "up", "down");

    if (inputDir != Vector2.Zero)
    {
      body.Rotation = camera.Rotation;

      Vector3 direction = (body.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

      velocity.X = direction.X * Speed;
      velocity.Z = direction.Z * Speed;

      moveStoppedSended = false;
    }
    else
    {
      velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
      velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
    }

    Velocity = velocity;

    MoveAndSlide();

    time += (float)delta;

    if (Velocity == Vector3.Zero)
    {
      if (!moveStoppedSended)
      {
        SendMoveStopped();
        moveStoppedSended = true;
      }

    } else if (time > 1.0f/20.0f)
    {
      SendMoving();

      time = 0.0f;
    }
  }

  void SendMoving()
  {
    RpcId(1, "SendMovement", new Vector2(GlobalPosition.X, GlobalPosition.Z), GetActorRotation().Y);
  }

  void SendMoveStopped()
  {
    RpcId(1, "SendMovementStopped", new Vector2(GlobalPosition.X, GlobalPosition.Z), GetActorRotation().Y);
  }

  void _AuthorityController(double delta)
  {
    _RotateCamera(delta);
    _MoveCharacter(delta);
    InputSkill();
  }
}
