using Godot;
using System;
using System.Collections.Generic;

class SkillHandler : IComponent
{
  Node actor;

  List<Skill> skills;

  public SkillHandler(Node actor)
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

    var instance = skill.Scene.Instantiate();

    actor.CallDeferred("add_child", instance);
  }
}
