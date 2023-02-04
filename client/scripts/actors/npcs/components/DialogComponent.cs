using Godot;
using DialogueManagerRuntime;

public class Dialog : IComponent
{
  Resource dialog;

  public void InputHandler(InputEvent @event)
  {
    throw new System.NotImplementedException();
  }

  public void Update(float delta)
  {
    throw new System.NotImplementedException();
  }

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
