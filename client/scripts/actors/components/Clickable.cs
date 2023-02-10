using Godot;

partial class Clickable : IComponent
{
  CharacterActor actor;

  static PackedScene Scene = ResourceLoader.Load<PackedScene>("res://components/clickable.tscn");

  public Clickable(CharacterActor actor)
  {
    this.actor = actor;

    var instance = Scene.Instantiate<Area3D>();

    actor.AddChild(instance);

    instance.InputEvent += InputEvent;
  }

  void InputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
  {
    if (@event is InputEventMouseButton)
    {
      if (@event.IsPressed())
      {
        actor.EmitSignal(CharacterActor.SignalName.ActorClicked);
      }
    }
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}