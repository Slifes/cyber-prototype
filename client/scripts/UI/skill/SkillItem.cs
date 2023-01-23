using Godot;

partial class SkillItem: Control
{
  [Export]
  public Skill skill;

  public override void _Ready()
  {
	  GetNode<ColorRect>("ColorRect").Color = skill.iconColor;
  }

  public override Variant _GetDragData(Vector2 atPosition)
  {
	var self = (Control)Duplicate();

	self.GetNode<ColorRect>("ColorRect").Size = new Vector2(50, 50);
	self.GetNode<ColorRect>("ColorRect").SetAnchorsPreset(LayoutPreset.TopLeft);

	SetDragPreview(self);

	return this;
  }
}
