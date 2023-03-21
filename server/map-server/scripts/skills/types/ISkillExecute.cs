using Godot;

interface ISkillExecute
{
  static abstract void Execute(IActorZone actor, Skill skill);
}
