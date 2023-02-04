using Godot;
using System.Collections.Generic;

public enum AIState
{
  Idle,
  Walking,
  Steering,
  Attacking,
  Died
}

class BehaviorComponent : IComponent
{
  AIState state = AIState.Idle;

  IBehavior behavior;

  Dictionary<AIState, IBehavior> behaviors;

  BaseNpcActor actor;

  public BehaviorComponent(BaseNpcActor actor)
  {
    this.actor = actor;

    behaviors = new()
    {
      // {AIState.Steering, new BasedContextSteering(this)},
      // {AIState.Walking, new BaseMovement(this)},
      // {AIState.Attacking, new BaseAttack(this)},
      // {AIState.Died, new BaseDied(this)}
    };

    ChangeState(AIState.Walking);
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta)
  {
    if (behavior != null)
    {
      behavior.Handler(delta);
    }
  }

  public void ChangeState(AIState state, Variant data = new Variant())
  {
    GD.Print("New state: ", state);

    if (behavior != null)
    {
      behavior.Finish();
    }

    behavior = behaviors[state];
    behavior.SetData(data);
    behavior.Start();

    this.state = state;
  }
}
