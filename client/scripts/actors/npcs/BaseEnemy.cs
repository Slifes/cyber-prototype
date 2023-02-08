using Godot;
using System.Collections.Generic;

partial class BaseEnemy : BaseNPC
{
  [Signal]
  public delegate void SvBehaviorSetStateEventHandler(Variant state, Variant position, Variant yaw, Variant data);

  [Signal]
  public delegate void SvBehaviorUpdateStateEventHandler(Variant state, Variant position, Variant yaw, Variant data);

  public AnimationPlayer Animation { get; set; }

  public List<int> skills;

  public override void _Ready()
  {
    onActorReady();

    SetProcessUnhandledInput(false);
    SetProcessInput(false);
    SetProcessShortcutInput(false);

    Animation = GetNode<AnimationPlayer>("AnimationPlayer");

    components = CreateComponents();

  }

  protected override IComponent[] CreateComponents()
  {
    return new IComponent[5]
    {
      new ActorHover(this),
    new MiniHPBar(this),
    new DamageLabel(this),
    new AgressiveBehavior(this),
    new NpcSkillHandler(this)
    };
  }
}
