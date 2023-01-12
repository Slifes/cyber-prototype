using Godot;

class BaseMovement: IBehavior
{
  Vector3 targetPosition;

  FastNoiseLite noise = ResourceLoader.Load<FastNoiseLite>("res://noise/movement.tres");

  float time = 0.0f;

  float MaxDistance = 0.5f;

  int nsignal = 1;

  public void Start(Npc actor)
  {

  }

  private Vector3 GetDirection(float time)
  {
    var offsetX = noise.GetNoise3d(time, 0, 0);
    var offsetZ = noise.GetNoise3d(0,0,time);

    Vector3 velocity = new Vector3(offsetX, 0, offsetZ);

    return velocity.Normalized();
  }

  private float GetFunctionX()
  {
    return Mathf.PingPong((float)Time.GetUnixTimeFromSystem(), 8);
  }

  public void Handler(Npc actor, double delta)
  {
    GD.Print(GetFunctionX());

    var direction = GetDirection(time);

    actor.Velocity = direction * 10 * (float)delta;

    actor.MoveAndSlide();
  }

  public void Finish(Npc actor)
  {

  }
}