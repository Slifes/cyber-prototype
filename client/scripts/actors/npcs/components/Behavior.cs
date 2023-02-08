using Godot;
using System.Collections.Generic;

public enum AIState
{
  Walking,
  Steering,
  Attacking,
}

abstract class Behavior : IComponent
{
  protected AIState state = AIState.Walking;

  protected IBehavior behavior;

  protected Dictionary<AIState, IBehavior> behaviors;

  private BaseEnemy _actor;

  public BaseEnemy Actor { get { return _actor; } }

  public Behavior(BaseEnemy actor)
  {
    _actor = actor;

    _actor.SvBehaviorSetState += BehaviorSetState;
    _actor.SvBehaviorUpdateState += BehaviorUpdateState;
  }

  void BehaviorSetState(Variant state, Variant position, Variant yaw, Variant data)
  {
    Actor.GlobalPosition = (Vector3)position;
    Actor.Rotation = new Vector3(0, (float)yaw, 0);

    this.ChangeState((AIState)(int)state, data);
  }

  void BehaviorUpdateState(Variant state, Variant position, Variant yaw, Variant data)
  {
    Actor.GlobalPosition = (Vector3)position;
    Actor.Rotation = new Vector3(0, (float)yaw, 0);

    if (behavior != null)
    {
      this.behavior.SetData(data);
    }
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
