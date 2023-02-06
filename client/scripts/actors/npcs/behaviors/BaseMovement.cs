using Godot;

class BaseMovement : IBehavior
{
  Vector3 targetPosition;

  static FastNoiseLite noise = ResourceLoader.Load<FastNoiseLite>("res://noise/movement.tres");

  float time = 0.0f;

  float MaxDistance = 20f;

  int nsignal = 1;

  float count = 0;

  Behavior actor;

  public BaseMovement(Behavior actor)
  {
    this.actor = actor;
  }

  public void Start()
  {
    actor.Actor.Animation.Play("Walking");
  }

  private Vector3 GetDirection(float time)
  {
    var offsetX = noise.GetNoise3D(time, 0, 0);
    var offsetZ = noise.GetNoise3D(0, 0, time);

    Vector3 velocity = new Vector3(offsetX, 0, offsetZ);

    return velocity.Normalized() * MaxDistance;
  }

  private float GetFunctionX()
  {
    var time = Time.GetUnixTimeFromSystem();

    var t = time - (float)((int)(time / 10.0f)) * 10;

    return Mathf.Abs(((int)t / 10.0f));
  }

  public void Handler(double delta)
  {
    /*var d = GetFunctionX();

    var direction = GetDirection(d);

    GD.Print("d: ", d);
    GD.Print("Direction: ", direction);
    GD.Print("Velocity: ", direction * (float)delta);

    actor.LinearVelocity = direction * (float)delta;*/
  }

  public void Finish() { }

  public void SetData(Variant data)
  {

  }
}
