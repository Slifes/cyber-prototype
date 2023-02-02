using Godot;
using DialogueManagerRuntime;

partial class BaseNpcActor : CharacterActor
{
  [Export]
  Resource Dialog;

  Decal hoverDecal;

  Area3D area;

  bool MouseHover;

  public override void _Ready()
  {
    hoverDecal = GetNode<Decal>("Selected");

    area = GetNode<Area3D>("Area3D");

    area.MouseEntered += AreaMouseEntered;
    area.MouseExited += AreaMouseExited;
  }

  void AreaMouseEntered()
  {
    GD.Print("MouseEntered");

    MouseHover = true;

    hoverDecal.Visible = true;

    Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
  }

  void AreaMouseExited()
  {
    GD.Print("MouseExited");
    MouseHover = false;

    hoverDecal.Visible = false;

    Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
  }

  public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
  {
    if (@event is InputEventMouseButton)
    {
      var inputEvent = (InputEventMouseButton)@event;

      if (inputEvent.IsPressed() && inputEvent.ButtonMask == MouseButtonMask.Left)
      {
        DialogueManager.ShowExampleDialogueBalloon(Dialog);
      }
    }
  }
}
