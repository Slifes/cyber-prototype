using Godot;

partial class TalkerComponent : IComponent
{
  static PackedScene scene = ResourceLoader.Load<PackedScene>("res://components/microphone.tscn");

  CharacterActor actor;

  int RecordBusIndex;

  AudioEffectRecord RecordEffect;

  AudioStreamWav RecordStream;

  AudioStreamPlayer3D Player;

  Sprite3D TalkingSprite;

  public TalkerComponent(CharacterActor actor)
  {
    this.actor = actor;

    Node voipScene = scene.Instantiate();

    actor.GetNode("Body").AddChild(voipScene);

    Player = voipScene.GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

    TalkingSprite = voipScene.GetNode<Sprite3D>("Talking");

    RecordBusIndex = AudioServer.GetBusIndex("Record");
    GD.Print("RecordBusIndex: ", RecordBusIndex);
    RecordEffect = (AudioEffectRecord)AudioServer.GetBusEffect(RecordBusIndex, 0);
  }

  public void InputHandler(InputEvent @event)
  {
    if (@event is InputEventKey keyEvent)
    {
      if (@event.IsActionPressed("voip_active"))
      {
        StartRecording();
      }

      if (@event.IsActionReleased("voip_active"))
      {
        StopRecording();
      }
    }
  }

  public void Update(float delta) { }

  void StartRecording()
  {
    GD.Print("Recording");
    TalkingSprite.Visible = true;
    RecordEffect.SetRecordingActive(true);
  }

  void StopRecording()
  {
    GD.Print("Stop Recording");
    RecordEffect.SetRecordingActive(false);

    RecordStream = RecordEffect.GetRecording();
    TalkingSprite.Visible = false;

    GD.Print("RecordStream: ", RecordStream.Data.Length);

    NetworkManager.Instance.SendPacket(new Packets.Client.CMAudioVoiceData
    {
      data = RecordStream.Data
    });
  }
}
