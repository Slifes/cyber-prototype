using Godot;
using Packets.Server;

partial class PacketManager
{
  void OnServerTime(IServerCommand command)
  {
    var pck = (ServerTime)command;

    var delay = (Time.GetUnixTimeFromSystem() * 1000.0) - pck.ClientTime;

    NetworkManager.Instance.FirstPickTick = Time.GetTicksMsec();
    NetworkManager.Instance.ServerTickSent = ((ServerTime)command).Time + (ulong)(delay / 2);
  }
}
