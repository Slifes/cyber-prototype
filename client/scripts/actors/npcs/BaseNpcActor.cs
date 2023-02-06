using Godot;
using System.Collections.Generic;

enum ComponentType
{
  Agressive,
  Dialogue
}

partial class BaseNpcActor : CharacterActor
{
  [Export]
  public int ID;

  [Export]
  public string ActorName;

  [Export]
  public ComponentType Type;

  [Export]
  public Resource Dialogue;

  public AnimationPlayer Animation { get; set; }

  public List<int> skills;

  public override void _Ready()
  {
	SetProcessUnhandledInput(false);
	SetProcessInput(false);
	SetProcessShortcutInput(false);

	Animation = GetNode<AnimationPlayer>("AnimationPlayer");

  StartComponents(Type);

  }

  protected virtual IComponent[] CreateComponents(ComponentType component)
  {
	return new IComponent[2]
	{
	  new ActorHover(this),
	  GetComponent(component)
	};
  }

  public void StartComponents(ComponentType component)
  {
	components = CreateComponents(component);
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
