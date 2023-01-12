using Godot;

using System.Collections.Generic;

enum NpcState
{
  Idle,
  Walking,
  Steering,
  Attacking,
  Died
}

partial class Npc: Actor
{
  NpcState state = NpcState.Idle;

  IBehavior behavior;

  Dictionary<NpcState, IBehavior> behaviors;

  public override void _Ready()
  {
	behaviors = new()
	{
	  {NpcState.Walking, new BasedContextSteering()}
	};

	ChangeState(NpcState.Walking);
  }

  public override void _PhysicsProcess(double delta)
  {
	if (behavior != null)
	{
	  behavior.Handler(this, delta);
	}
  }

  public void ChangeState(NpcState state)
  {
	if (behavior != null){
	  behavior.Finish(this);
	}

	behavior = behaviors[state];

	behavior.Start(this);
  }
}
