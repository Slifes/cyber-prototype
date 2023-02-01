using Godot;

partial class DragHelper: Control
{
	public override void _Ready()
	{
		this.SetAnchorsPreset(LayoutPreset.FullRect);
		this.SizeFlagsHorizontal = SizeFlags.ExpandFill;
		this.SizeFlagsVertical = SizeFlags.ExpandFill;
		this.Size = new Vector2(85, 85);
	}

	public void AddData(Control data)
	{
		var self = data.Duplicate();

		self.Name = "Data";

		AddChild(self);
	}

	public Control GetData()
	{
		return GetNode<Control>("Data");
	}

	public override Variant _GetDragData(Vector2 atPosition)
	{
		var data = (Control)GetNode("Data");

		var self = (Control)data.GetNode("View").Duplicate();

		self.SetAnchorsPreset(LayoutPreset.TopLeft);
		self.Size = new Vector2(50, 50);

		SetDragPreview(self);

		return data;
	}

  public override void _Input(InputEvent @event)
  {
		if(@event is InputEventMouseButton)
		{
			var inputEvent = (InputEventMouseButton)@event;

			if (inputEvent.DoubleClick)
			{
				GD.Print("Double click");
			}
		}
  }
}