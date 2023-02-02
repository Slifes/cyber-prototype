using Godot;
using System;
using System.Collections.Generic;


partial class Player
{
  List<Skill> skills;

  public List<Skill> Skills { get { return skills; } }

  public override void ExecuteSkill(Variant skillId)
  {
    int id = skillId.AsInt32();

    Skill skill = SkillManager.Instance.Get(id);

    if (skill.Effect != null)
    {
      var instance = skill.Effect.Instantiate();

      ((ISkillEffect)instance).SetOwner(this);

      GetNode("/root/World/Effects").AddChild(instance);

      ((ISkillEffect)instance).SetEffectRotation(GetBodyRotation());
      ((ISkillEffect)instance).SetEffectPosition(GlobalPosition);
    }

    string animationName = String.Format("Skills/{0}", id);

    if (Animation.HasAnimation(animationName))
    {
      Animation.Stop(true);
      Animation.Play(animationName);
    }

    if (IsMultiplayerAuthority())
    {
      SkillControl.Instance.UpdateSkillItems(id, 0);
    }
  }

  private List<Skill> LoadSkills(List<int> skillsToBeLoaded)
  {
    var animationLibrary = Animation.GetAnimationLibrary("Skills");

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

  public void SendRequestSkill(Variant id, Variant data)
  {
    RpcId(1, "RequestSkill", id, data);
  }
}
