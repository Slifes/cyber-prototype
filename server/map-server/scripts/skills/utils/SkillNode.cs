﻿using Godot;

partial class SkillNode : Node3D
{
  private static SkillNode instance;

  public override void _Ready()
  {
    if (Multiplayer.IsServer())
      instance = this;
  }

  public static void Spawn(AreaSkillBase areaSkill)
  {
    instance.CallDeferred("add_child", areaSkill);
  }
}
