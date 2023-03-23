using Godot;

partial class Zone
{
  public void SendActorConnected(SessionActor actor)
  {
    ((Player)actor).AddZone(this);

    nearests.CreateActorList(actor.GetActorId());

    Rpc("ActorConnected", actor.GetActorId(), (int)actor.GetActorType(), actor.Position, actor.Yaw);
  }

  public void SendActorDisconnected(SessionActor actor)
  {
    ((Player)actor).RemoveZone(this);

    nearests.RemoveActorList(actor.GetActorId());

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
