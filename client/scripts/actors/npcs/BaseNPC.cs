using Godot;
using System.Collections.Generic;

partial class BaseNPC : CharacterActor
{
  [Export]
  public int ID;

  [Export]
  public string ActorName;

  [Export]
  public Resource Dialogue;

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

  protected virtual IComponent[] CreateComponents()
  {
    return new IComponent[2]
    {
      new ActorHover(this),
    new Dialogue(this)
    };
  }
}
