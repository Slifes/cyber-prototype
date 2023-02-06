using Godot;
using System;
using System.Collections.Generic;

class SkillHandler : IComponent
{
  List<Skill> skills;

  AnimationPlayer animationPlayer;

  public SkillHandler(Node actor)
  {
    animationPlayer = actor.GetNode<AnimationPlayer>("AnimationPlayer");

    skills = new();
  }

  public void LoadSkill(List<int> dbSkills)
  {
    var animationLibrary = animationPlayer.GetAnimationLibrary("Skills");

    foreach (var id in dbSkills)
    {
      Skill skill = SkillManager.Instance.Get(id);

      if (skill != null)
      {
        skills.Add(skill);

        if (skill.Type == SkillType.Active && skill.animation != null)
        {
          animationLibrary.AddAnimation(skill.ID.ToString(), (Animation)skill.animation.Duplicate(true));
        }
      }
    }
  }

  public void ExecuteSkill(Variant id)
  {
    currentSkill = SkillManager.Instance.Get(id.AsInt32());

    if (currentSkill.animation != null)
    {
      var animationName = String.Format("Skills/{0}", id.ToString());

      if (animationPlayer.HasAnimation(animationName))
      {
        animationPlayer.Stop(true);
        animationPlayer.Play(String.Format("Skills/{0}", id.ToString()));
      }
    }

    if (currentSkill.Effect != null)
    {
      Node skillEffect = currentSkill.Effect.Instantiate();
      ISkillEffect effect = (ISkillEffect)skillEffect;

      effect.SetOwner(this);

      GetNode("/root/World/Effects").AddChild(skillEffect);

      effect.SetEffectRotation(this.Rotation);
      effect.SetEffectPosition(this.GlobalPosition);
    }
  }
}
