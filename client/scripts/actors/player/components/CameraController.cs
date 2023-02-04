using Godot;

struct ShakeCamera
{
  public float MaxX = 5;

  public float MaxY = 5;

  public float MaxR = 25;

  public float TimeScale = 150;

  public float Trauma = 0.0f;

  public float Time = 0.0f;

  public ShakeCamera() { }
}

class CameraController : IComponent
{
  static FastNoiseLite noise = ResourceLoader.Load<FastNoiseLite>("res://noise/shake_camera.tres");

  bool mouseCameraPressed;

  float MouseWheelVelocity = .5f;

  ShakeCamera shake = new();

  Vector2 mouseMoveCameraInitial = Vector2.Zero;

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

  public void ApplyTrauama(float value)
  {
    shake.Trauma = value;
  }

  public virtual void InputHandler(InputEvent @event)
  {
    if (!(@event is InputEventMouseButton)) return;

    InputEventMouseButton emb = (InputEventMouseButton)@event;

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
    RotateCamera(delta);
    Shake(delta);
  }

  void Shake(float delta)
  {
    shake.Time += delta;

    var shakeTrauma = Mathf.Pow(shake.Trauma, 2);
    var offsetX = noise.GetNoise3D(shake.Time * shake.TimeScale, 0, 0) * shake.MaxX * shakeTrauma;
    var offsetY = noise.GetNoise3D(0, shake.Time * shake.TimeScale, 0) * shake.MaxY * shakeTrauma;

    camera.HOffset = offsetX;
    camera.VOffset = offsetY;

    if (this.shake.Trauma > 0)
    {
      this.shake.Trauma = Mathf.Clamp(this.shake.Trauma - (delta * 0.6f), 0, 1);
    }
  }

  void RotateCamera(float delta)
  {
    bool isPressed = Input.IsMouseButtonPressed(MouseButton.Right);

    if (isPressed)
    {
      if (mouseCameraPressed)
      {
        Vector2 currentMousePosition = actor.GetViewport().GetMousePosition();

        float x = mouseMoveCameraInitial.X - currentMousePosition.X;

        float velocity = 0.2f * x * delta;

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
