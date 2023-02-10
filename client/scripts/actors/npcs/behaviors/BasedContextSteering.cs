using Godot;
using System.Collections.Generic;

class BasedContextSteering : IBehavior
{
  static PackedScene steering = ResourceLoader.Load<PackedScene>("res://components/based_context_steering.tscn");

  RayCast3D[] raycasts;

  Vector3[] rayDirections;

  Behavior actor;

  Node rays;

  Vector3 TargetPosition;

  public BasedContextSteering(Behavior actor)
  {
    this.actor = actor;

    rays = steering.Instantiate();

    actor.Actor.AddChild(steering.Instantiate());
  }

  public void Start()
  {
    raycasts = new RayCast3D[rays.GetChildCount()];
    rayDirections = new Vector3[raycasts.Length];

    for (var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i] = (RayCast3D)rays.GetChild(i);
      raycasts[i].Enabled = true;
    }
  }

  private Vector3 GetDirection()
  {
    for (var i = 0; i < raycasts.Length; i++)
    {
      rayDirections[i] = raycasts[i].TargetPosition.Rotated(Vector3.Up, actor.Actor.Rotation.Y);
    }

    var interestMap = new List<float>(new float[raycasts.Length]);

    var targetPos = TargetPosition;

    for (var i = 0; i < raycasts.Length; i++)
    {
      var ray = raycasts[i];

      Vector3 toTarget = targetPos - actor.Actor.GlobalPosition;

      if (!ray.IsColliding())
      {
        interestMap[i] = Mathf.Max(0, toTarget.Dot(rayDirections[i]));
      }
      else
      {
        interestMap[i] = 0.0f;
      }
    }

    // var interestBiggestInterest = interestMap.Max();
    // var indexOfInterestBigger = interestMap.FindIndex(x => x == interestBiggestInterest);

    Vector3 dir = Vector3.Zero;

    for (var i = 0; i < rayDirections.Length; i++)
    {
      dir += rayDirections[i] * interestMap[i];
    }

    return dir.Normalized();// rayDirections[indexOfInterestBigger];
  }

  public void Handler(double delta)
  {
    var t = Time.GetTicksUsec();

    Vector3 dir = GetDirection();

    Vector3 desiredVelocity = dir * 20.0f * (float)delta;

    actor.Actor.Velocity = desiredVelocity;

    actor.Actor.MoveAndSlide();

    actor.Actor.LookAt(TargetPosition);
  }

  public void Finish()
  {
    actor.Actor.Velocity = Vector3.Zero;

    for (var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i].Enabled = false;
    }
  }

  public void SetData(Variant data)
  {
    var dataArray = (Godot.Collections.Array<Variant>)data;

    if (dataArray.Count > 0)
    {
      TargetPosition = (Vector3)dataArray[0];
    }
  }
}
