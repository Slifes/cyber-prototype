using Godot;
using System.Collections.Generic;

class AgressiveBehavior : Behavior
{
  public AgressiveBehavior(BaseEnemy actor) : base(actor)
  {
    behaviors = new()
    {
      {AIState.Walking, new BaseMovement(this)},
      {AIState.Steering, new BasedContextSteering(this)},
      {AIState.Attacking, new BaseAttack(this)},
    };

    ChangeState(AIState.Walking);
  }
}
