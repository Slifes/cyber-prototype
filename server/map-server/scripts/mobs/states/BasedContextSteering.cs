using Godot;
using System.Collections.Generic;

class BasedContextSteering: IBehavior
{
  RayCast3D[] raycasts;

  Vector3[] rayDirections;

  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

  Npc actor;

  public BasedContextSteering(Npc actor)
  {
    this.actor = actor;
  }

  public void Start()
  {
    actor.AttackArea.SetDeferred("set_monitoring", true);
    actor.AttackArea.BodyEntered += AttackBodyEntered;

    actor.AgressiveArea.BodyExited += TargetBodyExited;

    var rays = actor.GetNode<Node3D>("Steering");

    raycasts = new RayCast3D[rays.GetChildCount()];
    rayDirections = new Vector3[raycasts.Length];

    for(var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i] = (RayCast3D)rays.GetChild(i);
      raycasts[i].Enabled = true;
    }
  }

  private void TargetBodyExited(Node3D node)
  {
    actor.ChangeState(NpcState.Walking);
  }

  private void AttackBodyEntered(Node3D node)
  {
    actor.AttackArea.BodyEntered -= AttackBodyEntered;

    actor.ChangeState(NpcState.Attacking);
  }

  private Vector3 GetDirection()
  {
    for(var i = 0; i < raycasts.Length; i++)
    {
      rayDirections[i] = raycasts[i].TargetPosition.Rotated(Vector3.Up, actor.Rotation.y);
    }

    var interestMap = new List<float>(new float[raycasts.Length]);
    var player = actor.Target;

    if (player == null){
      return Vector3.Zero;
    }
    
    var targetPos = player.GlobalPosition;

    for (var i = 0; i < raycasts.Length; i++)
    {
      var ray = raycasts[i];

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

    actor.LinearVelocity = desiredVelocity;

    actor.LookAt(actor.Target.GlobalPosition);

    actor.UpdateNPCState();
  }

  public void Finish()
  {
    actor.AgressiveArea.SetDeferred("set_monitoring", false);
    actor.LinearVelocity = Vector3.Zero;

    for(var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i].Enabled = false;
    }
  }

  public Variant GetData()
  {
    return new Godot.Collections.Array<Variant>()
    {
      actor.Target.GlobalPosition
    };
  }
}