using Godot;

partial class SkillShortcut: Control
{
  SkillSlot[] slots;

  Skill[] skills;

  public override void _Ready()
  {
    var hBox = GetNode<Control>("ColorRect/MarginContainer/HBoxContainer");

    slots = new SkillSlot[hBox.GetChildCount()];

    skills = new Skill[hBox.GetChildCount()];

    for (var i = 0; i < slots.Length; i++)
    {
      slots[i] = (SkillSlot)hBox.GetChild(i);
      slots[i].SkillChanged += (Skill skill, int index) => { skills[index] = skill; };
    }
  }

  public Skill GetSkillBySlot(int index)
  {
    return skills[index];
  }
}