using Godot;

class BuffSkill : ISkillExecute
{
  public static void Execute(IActorZone actor, Skill skill)
  {
    var actorNode = (Node3D)actor;

    var instance = skill.Scene.Instantiate<Heal>();

    instance.Actor = actor;

    actorNode.AddChild(instance);
  }
}
