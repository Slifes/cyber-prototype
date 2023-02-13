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
    Rpc("ActorConnected", actor.GetActorId(), (int)actor.GetActorType(), actor.Position, actor.Yaw);
  }

  public void SendActorDisconnected(SessionActor actor)
  {
    Rpc("ActorDisconnected", actor.GetActorId());
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActorConnected(int actorId, int actorType, Vector3 position, float yaw)
  {
    spawner.Spawn(actorId, position, yaw);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActorDisconnected(int actorId)
  {
    spawner.Despawn(actorId);
  }
}
