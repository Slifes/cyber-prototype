using Godot;

class SpeakerComponent : IComponent
{
  static PackedScene speakerScene = ResourceLoader.Load<PackedScene>("res://components/voip_speaker.tscn");

  AudioStreamPlayer3D speaker;

  Sprite3D TalkingSprite;

  public SpeakerComponent(Player actor)
  {
    var instance = speakerScene.Instantiate();

    actor.GetNode("Body").AddChild(instance);

    // speaker = instance.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
    // TalkingSprite = instance.GetNode<Sprite3D>("Talking");

    // actor.VoiceReceived += OnVoiceReceived;

    // speaker.Finished += () => { TalkingSprite.Visible = false; };
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
