using System;
using System.Collections.Generic;
using MessagePack;
using Godot;
using Packets.Server;

partial class PacketManager
{
  Dictionary<Type, Action<IServerCommand>> handlers;

  public PacketManager()
  {
    handlers = new()
    {
      {typeof(SMServerTime), OnServerTime},
      {typeof(SMActorEnteredZone), OnActorEnteredZone},
      {typeof(SMActorExitedZone), OnActorExitedZone},
      {typeof(SMActorDroppedItems), OnActorDrop},
      {typeof(SMDroppedItemRemove), OnItemDroppedRemove},
      {typeof(SMActorExecuteSkill), OnActorExecuteSkill},
      {typeof(SMActorStartMove), OnActorStartMove},
      {typeof(SMActorStopMove), OnActorStopMove},
      {typeof(SMActorEffect), OnActorEffect},
      {typeof(SMInventoryAddItem), OnInventoryAddItem},
      {typeof(SMInventoryRemoveItem), OnInventoryRemoveItem}
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
      GD.Print(e.StackTrace);
    }
  }
}
