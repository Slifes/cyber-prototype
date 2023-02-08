using Godot;
using System.Collections.Generic;

class BasedContextSteering : IBehavior
{
  RayCast3D[] raycasts;

  Vector3[] rayDirections;

  Behavior behavior;

  bool changeToAttackState = false;

  Area3D AttackArea;

  Area3D AgressiveArea;

  Vector3 LastTargetPosition = Vector3.Zero;

  public BasedContextSteering(Behavior behavior)
  {
    this.behavior = behavior;

    AttackArea = behavior.Actor.GetNode<Area3D>("AttackArea");
    AgressiveArea = behavior.Actor.GetNode<Area3D>("AgressiveArea");
  }

  public void Start()
  {
    changeToAttackState = false;

    AttackArea.BodyEntered += AttackBodyEntered;
    AgressiveArea.BodyExited += TargetBodyExited;

    var rays = behavior.Actor.GetNode<Node3D>("Steering");

    raycasts = new RayCast3D[rays.GetChildCount()];
    rayDirections = new Vector3[raycasts.Length];

    for (var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i] = (RayCast3D)rays.GetChild(i);
      raycasts[i].Enabled = true;
    }
  }

  private void TargetBodyExited(Node3D node)
  {
    if (behavior.Target != null && node.Name == behavior.Target.Name)
    {
      behavior.ChangeState(AIState.Walking);
      behavior.Target = null;
    }
  }

  private void AttackBodyEntered(Node3D node)
  {
    if (node.Name == behavior.Target.Name)
    {
      AttackArea.BodyEntered -= AttackBodyEntered;

      changeToAttackState = true;
    }
  }

  private Vector3 GetDirection()
  {
    for (var i = 0; i < raycasts.Length; i++)
    {
      rayDirections[i] = raycasts[i].TargetPosition.Rotated(Vector3.Up, behavior.Actor.Rotation.Y);
    }

    var interestMap = new List<float>(new float[raycasts.Length]);
    var player = behavior.Target;

    if (player == null)
    {
      return Vector3.Zero;
    }

    var targetPos = player.GlobalPosition;

    for (var i = 0; i < raycasts.Length; i++)
    {
      var ray = raycasts[i];

      Vector3 toTarget = targetPos - behavior.Actor.GlobalPosition;

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
    if (changeToAttackState)
    {
      behavior.ChangeState(AIState.Attacking);
      return;
    }

    var t = Time.GetTicksUsec();

    Vector3 dir = GetDirection();

    Vector3 desiredVelocity = dir * 10.0f * (float)delta;

    behavior.Actor.Velocity = desiredVelocity;

    behavior.Actor.MoveAndSlide();

    behavior.Actor.LookAt(behavior.Target.GlobalPosition);

    if (LastTargetPosition != behavior.Target.GlobalPosition)
    {
      LastTargetPosition = behavior.Target.GlobalPosition;
      behavior.UpdateState();
    }
  }

  public void Finish()
  {
    behavior.Actor.Velocity = Vector3.Zero;

    AttackArea.BodyEntered -= AttackBodyEntered;
    AgressiveArea.BodyExited -= TargetBodyExited;

    for (var i = 0; i < raycasts.Length; i++)
    {
      raycasts[i].Enabled = false;
    }
  }

  public Variant GetData()
  {
    return new Godot.Collections.Array<Variant>()
    {
      LastTargetPosition
    };
  }
}
