using Godot;
using DialogueManagerRuntime;

class Dialogue : IComponent
{
  Resource dialog;

  public Dialogue(BaseNPC actor) { }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }

  // public void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
  // {
  //   if (@event is InputEventMouseButton)
  //   {
  //     var inputEvent = (InputEventMouseButton)@event;

  //     if (inputEvent.IsPressed() && inputEvent.ButtonMask == MouseButtonMask.Left)
  //     {
  //       DialogueManager.ShowExampleDialogueBalloon(dialog);
  //     }
  //   }
  // }
}
