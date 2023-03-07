using Godot;
using MessagePack;
using System;
using System.Collections.Generic;
using Packets.Server;

partial class PacketManager
{
  Dictionary<Type, Action<IServerCommand>> handlers;

  public PacketManager()
  {
    handlers = new()
    {
      {typeof(ServerTime), OnServerTime},
      {typeof(SMActorEnteredZone), OnActorEnteredZone},
      {typeof(SMActorExitedZone), OnActorExitedZone},
      {typeof(SMExecuteSkill), OnActorExecuteSkill},
      {typeof(SMActorStartMove), OnActorStartMove},
      {typeof(SMActorStopMove), OnActorStopMove},
      {typeof(SMActorDamage), OnActorTakeDamage}
    };
  }

  public void OnPacketHandler(long id, byte[] pck)
  {
    try
    {
      var command = MessagePackSerializer.Deserialize<IServerCommand>(pck);

      handlers[command.GetType()].Invoke(command);
    }
    catch (Exception e)
    {
      GD.Print(string.Format("Packet Unknow: {0}", e.Message));
    }
  }
}
