using Godot;
using System;
using System.Collections.Generic;

partial class Player
{
  public override void ExecuteSkill(Variant index)
  {
    int i = index.AsInt32();
  
    animationPlayer.Play(String.Format("Skills/{0}", i));
  }

  private void InputSkill()
  {
    if (Input.IsActionJustPressed("attack"))
    {
      SendRequestSkill(0);
    }
  }

  private List<Skill> LoadSkills(List<int> skillsToBeLoaded)
  {
    var animationLibrary = animationPlayer.GetAnimationLibrary("Skills");

    var skills = new List<Skill>(new Skill[skillsToBeLoaded.Count]);

    for (var i = 0; i < skillsToBeLoaded.Count; i++)
    {
      var skill = SkillManager.Instance.Get(skillsToBeLoaded[i]);

      if (skill != null)
      {
        if (skill.Type == SkillType.Active)
        {
          animationLibrary.AddAnimation(skill.ID.ToString(), skill.animation);
        }
      }

      skills[i] = skill;
    }

    return skills;
  }

  public void SendRequestSkill(Variant id)
  {
    RpcId(1, "RequestSkill", id);
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant id) { }
}
