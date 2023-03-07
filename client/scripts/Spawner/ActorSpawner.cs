using Godot;
using Packets.Server;

partial class ActorSpawner : Node3D, IActorSpawner
{
  public void Despawn(Packets.Server.SMActorExitedZone command)
  {
    var name = command.ActorId.ToString();

    if (HasNode(name))
    {
      GetNode(name).QueueFree();
    }
  }

  public virtual IActor Spawn(SMActorEnteredZone command) { return null; }
}
