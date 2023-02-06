using Godot;

using System;
using System.Collections.Generic;

enum AIState
{
  Idle,
  Walking,
  Steering,
  Attacking,
  Died
}

class Behavior
{
  AIState state = AIState.Idle;

  IBehavior behavior;

  Dictionary<AIState, IBehavior> behaviors;

  public Area3D AgressiveArea { get; set; }

  public Area3D AttackArea { get; set; }

  public Node3D Target { get; set; }

  public Behavior(BaseNPC actor)
  {
    AgressiveArea = GetNode<Area3D>("AgressiveArea");
    AttackArea = GetNode<Area3D>("AttackArea");

    behaviors = new()
    {
      {AIState.Walking, new BaseMovement(this)},
      {AIState.Steering, new BasedContextSteering(this)},
      {AIState.Attacking, new BaseAttack(this)},
    };

    ChangeState(AIState.Walking);
  }


  public void Update(double delta)
  {
    if (behavior != null)
    {
      behavior.Handler(delta);
    }
  }

  public void ChangeState(AIState state)
  {
    GD.Print("New state: ", state);

    if (behavior != null)
    {
      behavior.Finish();
    }

    behavior = behaviors[state];

    this.state = state;

    behavior.Start();
  }
}
