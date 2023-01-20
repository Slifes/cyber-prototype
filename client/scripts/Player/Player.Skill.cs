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
      GetNode<ServerBridge>("/root/World/Server").SendRequestSkill(0);
    }
  }

  private List<Skill> LoadSkills()
  {
    var skill = ResourceLoader.Load<Skill>("res://resources/skills/normal_attack.tres");

    var animationLibrary = animationPlayer.GetAnimationLibrary("Skills");
    
    GD.Print("AnimationID: ", skill.ID);
    GD.Print("Damage: ", skill.Damage);

    if (!animationLibrary.HasAnimation(skill.ID.ToString()))
      animationLibrary.AddAnimation(skill.ID.ToString(), skill.animation);
    
    return new()
    {
      skill,
    };
  }
}
