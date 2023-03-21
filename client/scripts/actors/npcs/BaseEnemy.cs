using System.Collections.Generic;

partial class BaseEnemy : BaseNPC
{
  public List<int> skills;

  public override void _Ready()
  {
    base._Ready();

    SetProcessUnhandledInput(false);
    SetProcessInput(false);
    SetProcessShortcutInput(false);
  }

  protected override IComponent[] CreateComponents()
  {
    return new IComponent[4]
    {
      new ActorHover(this),
      new MiniHPBar(this),
      new EffectComponent(this),
      new NpcSkillHandler(this)
    };
  }
}
