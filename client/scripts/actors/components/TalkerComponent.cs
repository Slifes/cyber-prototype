using Godot;

partial class TalkerComponent : IComponent
{
  static PackedScene scene = GD.Load<PackedScene>("res://components/voip_microphone.tscn");

  CharacterActor actor;

  public TalkerComponent(CharacterActor actor)
  {
    this.actor = actor;

    Node voipScene = scene.Instantiate();

    actor.GetNode("Body").AddChild(voipScene);
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }

}
