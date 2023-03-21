using Godot;

using System;
using System.Collections.Generic;

enum AIState
{
  Walking,
  Steering,
  Attacking,
  Died
}

abstract class Behavior
{
  AIState state = AIState.Walking;

  IBehavior behavior;

  protected Dictionary<AIState, IBehavior> behaviors;

  protected BaseEnemy actor;

  public BaseEnemy Actor { get { return actor; } }

  public ZoneActor Target { get; set; }

  public void Update(double delta)
  {
    if (behavior != null)
    {
      behavior.Handler(delta);
    }
  }

  public void UpdateState()
  {
    // ServerBridge.Instance.SendNpcUpdateState(actor.GetPlayersId(), actor.GetActorId(), (int)state, actor.GlobalPosition, actor.Rotation.Y, GetData());
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

    // ServerBridge.Instance.SendNpcChangeState(actor.GetPlayersId(), actor.GetActorId(), (int)state, actor.GlobalPosition, actor.Rotation.Y, GetData());
  }

  public Variant GetData()
  {
    if (behavior == null)
    {
      return new Variant();
    }

    return behavior.GetData();
  }
}
