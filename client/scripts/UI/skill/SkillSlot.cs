using Godot;

partial class SkillSlot: Control
{
  [Export]
  public Skill skill;

  public override Variant _GetDragData(Vector2 atPosition)
  {
	if (skill == null)
	{
	  return base._GetDragData(atPosition);
	}

	var self = (SkillSlot)Duplicate();

	self.GetNode<ColorRect>("bkg").Color = skill.iconColor;
	self.GetNode<ColorRect>("bkg").Size = new Vector2(50, 50);
	self.GetNode<ColorRect>("bkg").SetAnchorsPreset(LayoutPreset.TopLeft);
  self.skill = (Skill)skill.Duplicate();

	SetDragPreview(self);

  // this.skill = null;
  this.GetNode<ColorRect>("bkg").Color = new Color("#828282");

	return this;
  }

  public override bool _CanDropData(Vector2 atPosition, Variant data)
  {
	var control = (Control)data;

	return true;
  }

  public override void _DropData(Vector2 atPosition, Variant data)
  {
	var skillItem = (Skill)((Control)data).Get("skill");

	skill = skillItem;

	GetNode<ColorRect>("bkg").Color = skill.iconColor;
  }
}
