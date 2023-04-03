using Godot;

partial class ShardArea : Area3D
{
  [Export]
  ShardTransport shard;

  Zone zone;

  public override void _Ready()
  {
    zone = shard.GetNode<Zone>("zone");

    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;
  }

  void OnBodyEntered(Node3D body)
  {
    SessionActor actor = (SessionActor)body;

    GD.Print("Send Actor to shard");

    zone.SendActorConnected(actor);
  }

  void OnBodyExited(Node3D body)
  {
    SessionActor actor = (SessionActor)body;

    zone.SendActorDisconnected(actor);
  }

  public override string[] _GetConfigurationWarnings()
  {
    if (shard == null)
    {
      return new string[] { "Shard is not set." };
    }

    return new string[] { };
  }
}
