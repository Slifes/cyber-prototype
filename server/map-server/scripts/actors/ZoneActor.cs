using Godot;
using Godot.Collections;

partial class ZoneActor: CharacterBody3D
{
  [Signal]
  public delegate void SkillListEventHandler(Array<int> skillsId);

  [Signal]
  public delegate void ExecuteSkillEventHandler(int skillId, Variant data);

  Ghosting ghosting;

  SkillHandler skillHandler;

  public override void _Ready()
  {
    ghosting = new Ghosting(this);

    skillHandler = new SkillHandler(this);
  }
}