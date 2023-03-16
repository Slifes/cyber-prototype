using System.Linq;
using Godot;
using Packets.Client;
using Database;

partial class PacketManager
{
  void OnEnterSessionMap(long peerId, Player actor, IClientCommand command)
  {
    var pck = (EnterSessionMap)command;

    using var db = new WorldContext();

    // var instance = db.Sessions
    //   .Where(x => x.AuthToken == pck.AuthToken)
    //   .First();

    // if (instance == null)
    // {
    //   Networking.Instance.Disconnect((int)peerId);
    //   return;
    // }

    // GD.Print("Session: ", instance);
    // GD.Print("SessionID: ", instance.AuthToken);

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

    Networking.Instance.SendPacket(peerId, new Packets.Server.SMServerTime
    {
      Time = Time.GetTicksMsec(),
      ClientTime = pck.ClientTime
    });
  }
}
