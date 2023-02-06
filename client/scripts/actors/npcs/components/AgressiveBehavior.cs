using Godot;
using System.Collections.Generic;

class AgressiveBehavior : Behavior
{
  public AgressiveBehavior(BaseNpcActor actor) : base(actor)
  {
    behaviors = new()
    {
      {AIState.Steering, new BasedContextSteering(this)},
      {AIState.Walking, new BaseMovement(this)},
      {AIState.Attacking, new BaseAttack(this)},
    };

    ChangeState(AIState.Walking);
  }
}
