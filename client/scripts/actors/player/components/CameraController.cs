using Godot;

class CameraController : IPlayerComponent
{
  bool mouseCameraPressed;

  Vector2 mouseMoveCameraInitial = Vector2.Zero;

  float MouseWheelVelocity = .5f;

  float MouseWheelUpLimit = 1.0f;

  float MouseWheelDownLimit = 10.0f;

  Camera3D camera = new();

  Node3D pivot = new();

  Player actor;

  public CameraController(Player player)
  {
    this.actor = player;

    camera = new Camera3D();
    pivot.AddChild(camera);

    camera.Fov = 70;
    camera.Near = 0.1f;
    camera.Far = 10000;
    camera.Position = new Vector3(0, 4, 1.5f);
    camera.RotationDegrees = new Vector3(-70, 0, 0);
    camera.Current = true;

    player.AddChild(pivot);
  }

  public virtual void InputHandler(InputEvent @event)
  {
    if (!(@event is InputEventMouseButton)) return;

    InputEventMouseButton emb = (InputEventMouseButton)@event;

    GD.Print(emb.ButtonIndex);

    if (emb.ButtonIndex == MouseButton.WheelUp)
    {
      camera.Position -= new Vector3(0, MouseWheelVelocity, 0.20f);
    }
    if (emb.ButtonIndex == MouseButton.WheelDown)
    {
      camera.Position += new Vector3(0, MouseWheelVelocity, 0.20f);
    }
  }

  public void Update(float delta)
  {
    bool isPressed = Input.IsMouseButtonPressed(MouseButton.Right);

    if (isPressed)
    {
      if (mouseCameraPressed)
      {
        Vector2 currentMousePosition = actor.GetViewport().GetMousePosition();

        float x = mouseMoveCameraInitial.X - currentMousePosition.X;

        float velocity = 0.2f * x * (float)delta;

        pivot.RotateY(velocity);

        mouseMoveCameraInitial = currentMousePosition;

        actor.Body.Rotation = pivot.Rotation;
      }
      else
      {
        mouseCameraPressed = true;
        mouseMoveCameraInitial = actor.GetViewport().GetMousePosition();
      }
    }
    else
    {
      mouseCameraPressed = false;
    }
  }
}
