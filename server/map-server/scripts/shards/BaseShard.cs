using Godot;

partial class BaseShard : Node
{
  protected ShardSpawner spawner;

  public override void _Ready()
  {
    spawner = GetNode<ShardSpawner>("spawner");
  }

  public void SendActorConnected(SessionActor actor)
  {
    ((Player)actor).AddZone(this);

    Rpc("ActorConnected", actor.GetActorId(), (int)actor.GetActorType(), actor.Position, actor.Yaw);
  }

  public void SendActorDisconnected(SessionActor actor)
  {
    // actor.RemoveZone(this);

    Rpc("ActorDisconnected", actor.GetActorId());
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActorConnected(int actorId, int actorType, Vector3 position, float yaw)
  {
    GD.Print("Actor Connected");

    spawner.Spawn(actorId, position, yaw);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActorDisconnected(int actorId)
  {
    GD.Print("Actor Disconnected");

    spawner.Despawn(actorId);
  }
}
