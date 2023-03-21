using Godot;
using Godot.Collections;

partial class BaseEnemy : BaseNPC
{
  [Export]
  public Array<Skill> skills;

  private Behavior behavior;

  private ActorState state;

  public override void _Ready()
  {
    base._Ready();

    behavior = new AgressiveBehavior(this);

    state = ActorState.Idle;

    Array<int> ids = new();

    foreach (var id in skills)
    {
      ids.Add(id.ID);
    }

    EmitSignal(ZoneActor.SignalName.SkillList, ids);
  }

  public override void _PhysicsProcess(double delta)
  {
    behavior.Update(delta);
  }
}
