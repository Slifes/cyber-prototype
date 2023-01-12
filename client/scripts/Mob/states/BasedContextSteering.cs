using Godot;
using System.Collections.Generic;
using System.Linq;

class BasedContextSteering: IBehavior
{
  RayCast3D[] raycasts;

  Actor target;

  public void Start(Npc actor)
  {
    var rays = actor.GetNode<Node3D>("Steering");

    raycasts = new RayCast3D[rays.GetChildCount()];

    for(var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i] = (RayCast3D)rays.GetChild(i);
      raycasts[i].Enabled = true;
    }
  }

  private Vector3 GetDirection(Actor actor)
  {
    var interestMap = new List<float>(new float[raycasts.Length]);
    var rayDirections = new List<Vector3>(new Vector3[raycasts.Length]);
    var player = (Node3D)actor.GetNode<Node3D>("/root/World/Players").GetChild(0);

    if (player == null){
      return Vector3.Zero;
    }
    
    var targetPos = player.GlobalPosition;

    for (var i = 0; i < raycasts.Length; i++)
    {
      var ray = raycasts[i];
      rayDirections[i] = ray.TargetPosition.Rotated(Vector3.Up, ray.Rotation.y);

      Vector3 toTarget = targetPos - actor.GlobalPosition;

      if(!ray.IsColliding())
      {
        interestMap[i] = Mathf.Max(0, toTarget.Dot(rayDirections[i]));
      } else 
      {
        interestMap[i] = 0.0f;
      }
    }

    // var interestBiggestInterest = interestMap.Max();
    // var indexOfInterestBigger = interestMap.FindIndex(x => x == interestBiggestInterest);

    Vector3 dir = Vector3.Zero;

    for (var i = 0; i < rayDirections.Count; i++)
    {
      dir += rayDirections[i] * interestMap[i];
    }

    return dir.Normalized();// rayDirections[indexOfInterestBigger];
  }

  public void Handler(Npc actor, double delta)
  {
    Vector3 dir = GetDirection(actor);

    Vector3 desiredVelocity = dir * 20.0f * (float)delta;

    actor.Velocity = desiredVelocity;

    actor.MoveAndSlide();
  }

  public void Finish(Npc actor)
  {
    for(var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i].Enabled = false;
    }
  }
}