using Godot;

partial class Usable : Item
{
  [Export]
  public int skillId;

  public Skill Skill { get { return SkillManager.Instance.Get(skillId); } }
}
