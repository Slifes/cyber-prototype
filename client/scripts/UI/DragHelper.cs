using Godot;

partial class DragHelper : Control
{
  public Vector2 FutureSize;

  public static DragHelper Create(Control node, Vector2 size)
  {
    var dragHelper = new DragHelper();

    dragHelper.AddData(node);
    dragHelper.FutureSize = size;

    return dragHelper;
  }

  public override void _Ready()
  {
    this.SetAnchorsPreset(LayoutPreset.FullRect);
    this.SizeFlagsHorizontal = SizeFlags.Fill;
    this.SizeFlagsVertical = SizeFlags.Fill;
    this.CallDeferred("Set");
  }

  void Set()
  {
    this.Size = FutureSize;
    this.CustomMinimumSize = FutureSize;
  }


  public void AddData(Control data)
  {
    data.Name = "Data";

    AddChild(data);
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

  public override void _GuiInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton)
    {
      var inputEvent = (InputEventMouseButton)@event;

      if (inputEvent.DoubleClick)
      {
        ((IUsable)GetData()).Use();
      }
    }
  }
}
