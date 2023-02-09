using Godot;
using System;
using System.Collections.Generic;

class SkillHandler : IComponent
{
  CharacterActor actor;

  List<Skill> skills;

  public SkillHandler(CharacterActor actor)
  {
    skills = new();

    this.actor = actor;

    LoadSkill(new List<int>() { 0, 1 });
  }

  public void LoadSkill(List<int> dbSkills)
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

    if (skill.Type == SkillType.Active) {
      var instance = skill.Scene.Instantiate<Node3D>();

      instance.Rotation = actor.Rotation;

      SkillNode.Spawn((AreaSkillBase)instance);

      instance.CallDeferred("set_global_position", actor.GlobalPosition);
    }
  }
}
