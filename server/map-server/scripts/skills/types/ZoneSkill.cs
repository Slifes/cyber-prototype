using Godot;

class ActiveSkill : ISkillExecute
{
  public static void Execute(IActorZone actor, Skill skill)
  {
    var instance = skill.Scene.Instantiate<AreaSkillBase>();

    var node = (Node3D)actor;

    instance.Actor = actor;
    instance.Rotation = node.Rotation;
    instance.Position = node.GlobalPosition;

    SkillNode.Spawn(instance);
  }
}
