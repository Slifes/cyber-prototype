using Packets.Server;

partial class PacketManager
{
  void OnActorEnteredZone(IServerCommand command)
  {
    var pck = (SMActorEnteredZone)command;

    Spawner.Instance.Spawn(pck);
  }

  void OnActorExitedZone(IServerCommand command)
  {
    var pck = (SMActorExitedZone)command;

    Spawner.Instance.Unspawn(pck);
  }
}