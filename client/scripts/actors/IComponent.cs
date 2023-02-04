using Godot;

interface IComponent
{
  void InputHandler(InputEvent @event);

  void Update(float delta);
}
