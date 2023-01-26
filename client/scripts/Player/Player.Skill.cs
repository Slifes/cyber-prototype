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

    if(skill.Effect != null)
    {
      var instance = skill.Effect.Instantiate();
      
      ((ISkillEffect)instance).SetOwner(this);
  
      GetNode("/root/World/Effects").AddChild(instance);
      
      ((ISkillEffect)instance).SetEffectRotation(GetActorRotation());
      ((ISkillEffect)instance).SetEffectPosition(GlobalPosition);
    }
    
    animationPlayer.Play(String.Format("Skills/{0}", id));

    if (IsMultiplayerAuthority()){
      SkillControl.Instance.UpdateSkillItems(id, 0);
    }
  }

  private void InputSkill()
  {
    for(var i = 0; i < 6; i++)
    {
      if (Input.IsActionJustPressed(String.Format("slot{0}", i)))
      {
        var skillItem = UIControl.Instance.GetSkillSlot(i);

        if (skillItem != null && skillItem.Available) {
          SendRequestSkill(skillItem.skill.ID);
        }
      }
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
