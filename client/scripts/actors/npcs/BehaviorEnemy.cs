using Godot;

partial class AgressiveEnemy : BaseNpcActor
{
  [Signal]
  public delegate void SvBehaviorSetStateEventHandler(Variant state, Variant position, Variant yaw, Variant data);

  [Signal]
  public delegate void SvBehaviorUpdateStateEventHandler(Variant state, Variant position, Variant yaw, Variant data);

  protected override IComponent[] CreateComponents(ComponentType component)
  {
	  return new IComponent[5]
    {
      new MiniHPBar(this),
      new ActorHover(this),
      new DamageLabel(this),
      new NpcSkillHandler(this),
      new AgressiveBehavior(this)
    };
  }
}
