using Godot;
using System.Collections.Generic;

partial class BaseShard : Node
{
  protected Dictionary<int, MiniActor> actors;

  protected Node3D spawns;

  public override void _Ready()
  {
    spawns = GetNode<Node3D>("actors");
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActorConnected(int actorId, int actorType, Vector3 position, float yaw)
  {
    var instance = new MiniActor();

    instance.Name = actorId.ToString();

    spawns.AddChild(instance);

    instance.Position = position;
    instance.Rotation = new Vector3(0, yaw, 0);

    actors.Add(actorId, instance);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void ActorDisconnected(int actorId)
  {
    MiniActor actor = actors[actorId];

    actors.Remove(actorId);

    actor.QueueFree();
  }
}
