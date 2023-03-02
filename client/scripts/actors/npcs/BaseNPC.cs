using Godot;
using System.Collections.Generic;

partial class BaseNPC : CharacterActor
{
  [Export]
  public int ID;

  [Export]
  public string ActorName;

  public AnimationPlayer Animation { get; set; }

  public override void _Ready()
  {
    onActorReady();

    Animation = GetNode<AnimationPlayer>("AnimationPlayer");

    components = CreateComponents();
  }

  protected virtual IComponent[] CreateComponents()
  {
    return new IComponent[1]
    {
      new ActorHover(this),
    };
  }

  public override ActorType GetActorType()
  {
    return ActorType.Npc;
  }
}
