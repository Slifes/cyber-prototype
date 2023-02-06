using Godot;
using System.Collections.Generic;

public enum AIState
{
  Idle,
  Walking,
  Steering,
  Attacking,
}

abstract class Behavior : IComponent
{
  protected AIState state = AIState.Idle;

  protected IBehavior behavior;

  protected Dictionary<AIState, IBehavior> behaviors;

  private BaseNpcActor _actor;

  public BaseNpcActor Actor { get { return _actor; } }

  public Behavior(BaseNpcActor actor)
  {
    _actor = actor;
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
