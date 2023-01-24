using Godot;

partial class SkillSlot: Control
{
  [Export]
  public SkillItem skill;

  [Signal]
  public delegate void SkillChangedEventHandler(SkillItem newSkill, int index);

  public override Variant _GetDragData(Vector2 atPosition)
  {
	if (skill == null)
	{
	  return base._GetDragData(atPosition);
	}

	var node = (SkillItem)GetNode("Data");

	var self = (Control)node.GetNode("View").Duplicate();

	self.Size = new Vector2(50, 50);
	self.SetAnchorsPreset(LayoutPreset.TopLeft);

	SetDragPreview(self);

	RemoveChild(GetNode("Data"));

	UpdateDraggedSkill(null);

	return node;
  }

  public override bool _CanDropData(Vector2 atPosition, Variant data)
  {
	var control = (SkillItem)data;

	return control.skill.Type == SkillType.Active;
  }

  public override void _DropData(Vector2 atPosition, Variant data)
  {
	var node = (Control)data;

	var skillItem = (SkillItem)node.Duplicate();

	skillItem.Name = "Data";

	AddChild(skillItem);

	UpdateDraggedSkill(skillItem);
  }

  void UpdateDraggedSkill(SkillItem skillItem)
  {
	skill = skillItem;

	EmitSignal(nameof(SkillChanged), skill, GetIndex());
  }
}
