using Godot;

partial class ShardArea: Area3D
{
  [Export]
  ShardConnect shard;

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
}