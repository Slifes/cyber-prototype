using Packets.Server;

partial class PacketManager
{
  void OnServerTime(IServerCommand command)
  {
    NetworkManager.Instance.ServerTick = (ulong)((ServerTime)command).Time;
  }
}