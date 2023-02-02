using Godot;

interface IPlayerComponent
{
  void InputHandler(InputEvent @event);

  void Update(float delta);
}
