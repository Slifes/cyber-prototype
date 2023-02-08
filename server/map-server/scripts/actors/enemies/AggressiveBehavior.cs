
class AgressiveBehavior : Behavior
{
  public AgressiveBehavior(BaseNPC actor)
  {
    this.actor = (BaseEnemy)actor;

    behaviors = new()
    {
      {AIState.Walking, new BaseMovement(this)},
      {AIState.Steering, new BasedContextSteering(this)},
      {AIState.Attacking, new BaseAttack(this)},
    };

    ChangeState(AIState.Walking);
  }
}
