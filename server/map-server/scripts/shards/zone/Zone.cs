using Godot;
using Packets.Server;

partial class Zone : Node
{
  static PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://actors/player_zone.tscn");

  ZoneSpawner spawner;

  Nearests nearests;

  DropItems dropItems;

  private static Zone _instance;

  public static Zone Instance { get { return _instance; } }

  public override void _Ready()
  {
    base._Ready();

    if (GetParent<ShardTransport>().IsServer)
    {
      _instance = this;

      spawner = GetNode<ZoneSpawner>("spawner");
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

  void SendPacketToAllInZone(IServerCommand command)
  {
    var peers = nearests.GetPeers();

    Networking.Instance.SendPacketToMany(peers, command);
  }
}
