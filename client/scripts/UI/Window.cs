using Godot;

partial class Window : Control
{
  Vector2 dragPosition;

  public override void _GuiInput(InputEvent @event)
  {
    if (@event is InputEventMouseButton)
    {
      if (@event.IsPressed())
      {
        dragPosition = GetGlobalMousePosition() - GetGlobalRect().Position;
      }
      else
      {
        dragPosition = Vector2.Zero;
      }
    }

    if (@event is InputEventMouseMotion && dragPosition != Vector2.Zero)
    {
      GlobalPosition = GetGlobalMousePosition() - dragPosition;
    }
  }
}
