using Godot;
using System.Collections.Generic;

class BasedContextSteering : IBehavior
{
  RayCast3D rayCast;

  Behavior behavior;

  Area3D AttackArea;

  Area3D AgressiveArea;

  Vector3 LastTargetPosition = Vector3.Zero;

  int rayCastTargetIndex = 0;

  List<float> interestMap;

  Vector3[] rayDirections;

  Vector3[] rayCastTargetPosition = new Vector3[8]
  {
    new Vector3(0, 0, 0.6f),
    new Vector3(0.6f, 0, 0),
    new Vector3(0, 0, -0.6f),
    new Vector3(0, -0.6f, 0),
    new Vector3(0.4f, 0, 0.4f),
    new Vector3(0.4f, 0, -0.4f),
    new Vector3(-0.4f, 0, -0.4f),
    new Vector3(-0.4f, 0, 0.4f)
  };

  public BasedContextSteering(Behavior behavior)
  {
    this.behavior = behavior;

    AttackArea = behavior.Actor.GetNode<Area3D>("AttackArea");
    AgressiveArea = behavior.Actor.GetNode<Area3D>("AgressiveArea");
    rayCast = behavior.Actor.GetNode<RayCast3D>("BasedContextSteering");
  }

  public void Start()
  {
    AttackArea.BodyEntered += AttackBodyEntered;
    AgressiveArea.BodyExited += TargetBodyExited;

    rayDirections = new Vector3[rayCastTargetPosition.Length];
    interestMap = new List<float>(new float[rayCastTargetPosition.Length]);

    rayCast.Enabled = true;
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

      behavior.ChangeState(AIState.Attacking);
    }
  }

  private Vector3 GetDirection()
  {
    var player = behavior.Target;

    if (player == null || !player.IsInsideTree())
    {
      return Vector3.Zero;
    }

    rayDirections[rayCastTargetIndex] = rayCast.TargetPosition.Rotated(Vector3.Up, behavior.Actor.Rotation.Y);

    var targetPos = player.GlobalPosition;

    Vector3 toTarget = targetPos - behavior.Actor.GlobalPosition;

    if (!rayCast.IsColliding())
    {
      interestMap[rayCastTargetIndex] = Mathf.Max(0, toTarget.Dot(rayDirections[rayCastTargetIndex]));
    }
    else
    {
      interestMap[rayCastTargetIndex] = 0.0f;
    }

    Vector3 dir = Vector3.Zero;

    for (var i = 0; i < rayDirections.Length; i++)
    {
      dir += rayDirections[i] * interestMap[i];
    }

    return dir.Normalized();
  }

  void NextRayCastTargetPosition()
  {
    if (rayCastTargetIndex < 7)
    {
      rayCastTargetIndex++;
    }
    else
    {
      rayCastTargetIndex = 0;
    }

    rayCast.TargetPosition = rayCastTargetPosition[rayCastTargetIndex];
  }

  public void Handler(double delta)
  {
    var t = Time.GetTicksUsec();

    Vector3 dir = GetDirection();

    Vector3 desiredVelocity = dir * 10.0f * (float)delta;

    behavior.Actor.Velocity = desiredVelocity;

    behavior.Actor.MoveAndSlide();

    behavior.Actor.LookAt(behavior.Target.Position);

    if (LastTargetPosition != behavior.Target.Position)
    {
      LastTargetPosition = behavior.Target.Position;
      behavior.UpdateState();
    }

    NextRayCastTargetPosition();
  }

  public void Finish()
  {
    behavior.Actor.Velocity = Vector3.Zero;

    AttackArea.BodyEntered -= AttackBodyEntered;
    AgressiveArea.BodyExited -= TargetBodyExited;

    rayCast.Enabled = false;
  }

  public Variant GetData()
  {
    return new Godot.Collections.Array<Variant>()
    {
      LastTargetPosition
    };
  }
}
