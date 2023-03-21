using Godot;
using Godot.Collections;

class SkillHandler
{
  ZoneActor actor;

  Array<Skill> skills;

  public Array<Skill> Skills { get { return skills; } }

  public SkillHandler(ZoneActor actor)
  {
    skills = new();

    this.actor = actor;

    actor.SkillList += LoadSkillList;
    actor.ExecuteSkill += ExecuteSkill;
  }

  void LoadSkillList(Array<int> skillsId)
  {
    LoadSkill(skillsId);

    GD.Print("Load SKill: ", skillsId);
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

  public void ExecuteSkill(int id, Variant data)
  {
    var skill = SkillManager.Instance.Get(id);

    switch (skill.Type)
    {
      case SkillType.Active:
        ActiveSkill.Execute(actor, skill);
        break;

      case SkillType.Buff:
        BuffSkill.Execute(actor, skill);
        break;
    }

    Zone.Instance.Rpc("ExecuteSkill", actor.GetActorID(), (int)actor.GetActorType(), id, data);
  }
}
