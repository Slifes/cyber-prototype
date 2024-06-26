﻿using Godot;
using System;

class NpcSkillHandler : IComponent
{
  BaseEnemy actor;

  public NpcSkillHandler(BaseEnemy actor)
  {
    this.actor = actor;

    actor.ExecuteSkill += ExecuteSkill;
  }

  void ExecuteSkill(Variant id)
  {
    GD.Print("NPC Execute Skill: ", id);
    actor.Animation.Play(String.Format("Skills/{0}", id.ToString()));
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
