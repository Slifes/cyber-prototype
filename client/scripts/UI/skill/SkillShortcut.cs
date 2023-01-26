using Godot;

partial class SkillShortcut: Control
{
  SkillSlot[] slots;

  SkillItem[] skills;

  public override void _Ready()
  {
    var hBox = GetNode<Control>("ColorRect/MarginContainer/HBoxContainer");

    slots = new SkillSlot[hBox.GetChildCount()];

    skills = new SkillItem[hBox.GetChildCount()];

    for (var i = 0; i < slots.Length; i++)
    {
      slots[i] = (SkillSlot)hBox.GetChild(i);
      slots[i].SkillChanged += (SkillItem skill, int index) => { skills[index] = skill; };
    }
  }

  public SkillItem GetSkillBySlot(int index)
  {
    return skills[index];
  }
}
