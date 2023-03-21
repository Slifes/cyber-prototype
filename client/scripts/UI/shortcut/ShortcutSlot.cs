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

    var node = GetNode("Data");

    var self = (Control)node.GetNode("View").Duplicate();

    self.Size = new Vector2(50, 50);
    self.SetAnchorsPreset(LayoutPreset.TopLeft);

    SetDragPreview(self);

    GetNode("Data").QueueFree();

    UpdateDraggedSkill(null);

    return node;
  }

  public override bool _CanDropData(Vector2 atPosition, Variant data)
  {
    var control = (Node)data;

    return control is IUsable;
  }

  public override void _DropData(Vector2 atPosition, Variant data)
  {
    var node = (Control)data;

    var item = node.Duplicate();

    item.Name = "Data";

    AddChild(item);

    UpdateDraggedSkill(item);
  }

  void UpdateDraggedSkill(Node item)
  {
    usable = (IUsable)item;

    EmitSignal(SignalName.Changed, (Node)item, GetIndex());
  }
}
