using Godot;
using Godot.Collections;

class SkillHandler
{
  ZoneActor actor;

  Array<Skill> skills;

  public SkillHandler(ZoneActor actor)
  {
    skills = new();

    this.actor = actor;

    actor.SkillList += LoadSkillList;
  }

  void LoadSkillList(Array<int> skillsId)
  {
    LoadSkill(skillsId);
  }

  public void LoadSkill(Array<int> dbSkills)
  {
    foreach (var id in dbSkills)
    {
      Skill skill = SkillManager.Instance.Get(id);

      if (skill != null)
      {
        skills.Add(skill);
      }
    }
  }

  public void ExecuteSkill(Variant id)
  {
    var skill = SkillManager.Instance.Get(id.AsInt32());

    if (skill.Type == SkillType.Active)
    {
      var instance = skill.Scene.Instantiate<Node3D>();

      instance.Rotation = actor.Rotation;

      SkillNode.Spawn((AreaSkillBase)instance);

      instance.CallDeferred("set_global_position", actor.GlobalPosition);
    }
  }
}
