using Godot;

partial class ToggleButton: Button
{
  [Export]
  Control target;

  public override void _Ready()
  {
    Pressed += ToggleEvent;
  }

  void ToggleEvent()
  {
    target.Visible = !target.Visible;
  }
}