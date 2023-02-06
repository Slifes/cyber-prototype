using Godot;
using System.Collections.Generic;

partial class BaseNpcActor : CharacterActor
{
  [Signal]
  public delegate void SvChangeStateEventHandler(Variant state, Variant position, Variant yaw, Variant data);

  public int ID;

  public AnimationPlayer Animation { get; set; }

  public List<int> skills;

  public override void _Ready()
  {
    SetProcessUnhandledInput(false);
    SetProcessInput(false);
    SetProcessShortcutInput(false);

    Animation = GetNode<AnimationPlayer>("AnimationPlayer");

    components = CreateComponents();
  }

  IComponent[] CreateComponents()
  {
    return new IComponent[3]
    {
      new MiniHPBar(this),
      new ActorHover(this),
      GetComponent(ComponentType.Dialogue)
    };
  }

  // public void ReceiveChangeState(Variant state, Variant position, Variant yaw, Variant data)
  // {
  // 	AIState _state = (AIState)(int)state;

  // 	GlobalPosition = (Vector3)position;
  // 	RotationDegrees = new Vector3(0, (float)yaw, 0);

  // 	ChangeState(_state, data);
  // }

  // public void ReceiveUpdateState(Variant state, Variant position, Variant yaw, Variant data)
  // {
  // 	GlobalPosition = (Vector3)position;
  // 	RotationDegrees = new Vector3(0, (float)yaw, 0);

  // 	if (behavior != null)
  // 	{
  // 		behavior.SetData(data);
  // 	}
  // }

  public IComponent GetComponent(ComponentType comp)
  {
    switch (comp)
    {
      case ComponentType.Agressive:
        return new AgressiveBehavior(this);

      case ComponentType.Dialogue:
        return new Dialogue(this);

      default:
        return null;
    }
  }
}
