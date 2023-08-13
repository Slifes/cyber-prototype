using Godot;
using System.Collections.Generic;

class BasedPathFinding : IBehavior
{
  Behavior behavior;

  NavigationAgent3D agent;

  Area3D AttackArea;

  Area3D AgressiveArea;

  Vector3 NextVelocity;

  public BasedPathFinding(Behavior behavior)
  {
    this.behavior = behavior;
    agent = behavior.Actor.GetNode<NavigationAgent3D>("NavigationAgent3D");
    AttackArea = behavior.Actor.GetNode<Area3D>("AttackArea");
    AgressiveArea = behavior.Actor.GetNode<Area3D>("AgressiveArea");

    agent.VelocityComputed += OnVelocityComputed;
  }

  public void Start()
  {
    AttackArea.BodyEntered += AttackBodyEntered;
    AgressiveArea.BodyExited += TargetBodyExited;
  }

  public void Finish()
  {
    AttackArea.BodyEntered -= AttackBodyEntered;
    AgressiveArea.BodyExited -= TargetBodyExited;
  }

  public Variant GetData()
  {
    return new Godot.Collections.Array()
    {
      agent.TargetPosition
    };
  }

  void OnVelocityComputed(Vector3 velocity)
  {
    behavior.Actor.Velocity = velocity;
    behavior.Actor.MoveAndSlide();

    Zone.Instance.Rpc("NpcMoving", behavior.Actor.GetActorID(), behavior.Actor.Position, behavior.Actor.Rotation.Y);
  }

  void CalculatePath(float delta, Vector3 position)
  {
    agent.TargetPosition = behavior.Target.GlobalPosition;

    float MovementSpeed = delta * 8.0f;

    Vector3 nextTargetPosition = agent.GetNextPathPosition();

    Vector3 velocity = (nextTargetPosition - behavior.Actor.GlobalPosition).Normalized() * MovementSpeed;

    agent.Velocity = velocity;
  }

  public void Handler(double delta)
  {
    CalculatePath((float)delta, behavior.Actor.Position);
  }

  private void TargetBodyExited(Node3D node)
  {
    if (behavior.Target != null && node.Name == behavior.Target.Name)
    {
      agent.Velocity = Vector3.Zero;
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
}
