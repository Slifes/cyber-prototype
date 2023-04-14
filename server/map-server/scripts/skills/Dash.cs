using Godot;

partial class Dash : Node, ISkillExecute
{
  ZoneActor actor;

  public static void Execute(IActorZone actor, Skill skill)
  {

    // this.actor.Velocity += actor.Direction * skill.Value;
  }

  public override void _PhysicsProcess(double delta)
  {

  }
}
