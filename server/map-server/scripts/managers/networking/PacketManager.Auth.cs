using Godot;
using Packets.Client;

partial class PacketManager
{
  void OnEnterSessionMap(long peerId, Player actor, IClientCommand command)
  {
    var pck = (EnterSessionMap)command;

    Player spawnedPlayer = (Player)PlayerSpawner.Instance.Spawn(peerId, Vector3.Up, new Godot.Collections.Array()
    {
      0,
      100,
      100,
      100,
      100
    });

    Networking.Instance.SendPacket(peerId, spawnedPlayer.GetActorPacket());
  }

  void OnFetchServerTime(long peerId, Player actor, IClientCommand command)
  {
    var pck = (FetchServerTime)command;

    Networking.Instance.SendPacket(peerId, new Packets.Server.ServerTime
    {
      Time = (long)Time.GetTicksMsec(),
    });
  }
}
