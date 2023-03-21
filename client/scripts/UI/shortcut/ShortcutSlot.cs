using Godot;

partial class ShortcutSlot : Control
{
  [Signal]
  public delegate void ChangedEventHandler(Node usable, int index);

  public IUsable usable;

  public override Variant _GetDragData(Vector2 atPosition)
  {
    if (usable == null)
    {
      return base._GetDragData(atPosition);
    }

    var node = GetData();

    var self = (Control)node.Duplicate();

    self.Size = new Vector2(50, 50);
    self.SetAnchorsPreset(LayoutPreset.TopLeft);

    SetDragPreview(self);

    RemoveChild(node);

    UpdateDraggedItem(null);

    return node;
  }

  public override bool _CanDropData(Vector2 atPosition, Variant data)
  {
    var control = (Control)data;

    return control is IUsable;
  }

  public override void _DropData(Vector2 atPosition, Variant data)
  {
    var node = (Control)data;

    var item = node.Duplicate();

    item.Name = "Data";

    AddChild(item);

    UpdateDraggedItem(item);
  }

  void UpdateDraggedItem(Node item)
  {
    usable = (IUsable)item;

    EmitSignal(SignalName.Changed, (Node)item, GetIndex());
  }

  public Control GetData()
  {
    return GetNode<Control>("Data");
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
