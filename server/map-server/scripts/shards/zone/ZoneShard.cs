using Godot;
using System.Collections.Generic;

partial class ZoneShard : BaseShard
{
  public Dictionary<int, List<int>> neraests;

  public override void _Ready()
  {
    neraests = new();
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(Variant actorId, Variant id, Variant type, Variant position, Variant yaw)
  {
    GD.Print("HUeEUHEU");
    if (!neraests.ContainsKey((int)id))
    {
      neraests.Add((int)actorId, new List<int>() { (int)id });
    }
    else
    {
      neraests[(int)actorId].Add((int)id);
    }
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(Variant actorId, Variant id, Variant type)
  {
    if (neraests.ContainsKey((int)actorId))
    {
      neraests[(int)actorId].Remove((int)id);
    }
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMoved(Variant actorId, Variant position, Variant yaw)
  {
    MiniActor actor = actors[(int)actorId];

    actor.GlobalPosition = (Vector3)position;
    actor.Rotation = new Vector3(0, (float)yaw, 0);
  }

  public override void _Process(double delta)
  {
    base._Process(delta);

    if (Multiplayer.IsServer())
      Rpc("ActorEnteredZone", 0, 0, 1, Vector3.Zero, 0.0f);

  }
}
