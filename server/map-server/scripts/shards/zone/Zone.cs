using Godot;
using Packets.Server;

partial class Zone : Node
{
  static PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://actors/player_zone.tscn");

  ShardSpawner spawner;

  Nearests nearests;

  DropItems dropItems;

  private static Zone _instance;

  public static Zone Instance { get { return _instance; } }

  public override void _Ready()
  {
    base._Ready();

    if (GetParent<ShardConnect>().IsServer)
    {
      _instance = this;

      spawner = GetNode<ShardSpawner>("spawner");
      dropItems = GetNode<DropItems>("items");
    }
    else
    {
      nearests = new();
    }
  }

  void SendPacketToAllNearest(int actorId, IServerCommand command)
  {
    Networking.Instance.SendPacketToMany(nearests.GetPlayerNearest(actorId), command);
  }

  void SendPacketToAllNearestAndMe(int actorId, IServerCommand command)
  {
    Networking.Instance.SendPacketToMany(actorId, nearests.GetPlayerNearest(actorId), command);
  }
}
