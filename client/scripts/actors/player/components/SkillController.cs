using Godot;
using System;
using System.Collections.Generic;

class SkillController : IComponent
{
  Player actor;

  List<Skill> skills;

  public List<Skill> Skills { get { return skills; } }

  public SkillController(Player actor)
  {
    this.actor = actor;

    this.actor.ExecuteSkill += ExecuteSkill;
  }

  public SkillController(Player actor, List<int> skills)
  {
    this.actor = actor;

    this.actor.ExecuteSkill += ExecuteSkill;

    this.skills = this.LoadSkills(skills);
  }

  public void ExecuteSkill(Variant skillId)
  {
    GD.Print("Executed skill");

    int id = skillId.AsInt32();

    Skill skill = SkillManager.Instance.Get(id);

    if (skill.Effect != null)
    {
      var instance = skill.Effect.Instantiate();

      ((ISkillEffect)instance).SetOwner(actor);

      actor.GetNode("/root/World/Effects").AddChild(instance);

      ((ISkillEffect)instance).SetEffectRotation(actor.GetBodyRotation());
      ((ISkillEffect)instance).SetEffectPosition(actor.GlobalPosition);
    }

    string animationName = String.Format("Skills/{0}", id);

    if (actor.Animation.HasAnimation(animationName))
    {
      actor.Animation.Stop(true);
      actor.Animation.Play(animationName);
    }
  }

  public List<Skill> LoadSkills(List<int> skillsToBeLoaded)
  {
    var animationLibrary = actor.Animation.GetAnimationLibrary("Skills");

    var skills = new List<Skill>(new Skill[skillsToBeLoaded.Count]);

    for (var i = 0; i < skillsToBeLoaded.Count; i++)
    {
      var skill = SkillManager.Instance.Get(skillsToBeLoaded[i]);

      if (skill != null)
      {
        if (skill.Type == SkillType.Active && skill.animation != null)
        {
          animationLibrary.AddAnimation(skill.ID.ToString(), skill.animation);
        }
      }

      skills[i] = skill;
    }

    return skills;
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
