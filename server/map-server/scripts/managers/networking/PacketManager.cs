using Godot;
using MessagePack;
using System;
using System.Collections.Generic;
using Packets.Client;

partial class PacketManager
{
  Dictionary<Type, Action<long, Player, IClientCommand>> handlers;

  SessionManager sessionManager;

  public PacketManager()
  {
    sessionManager = new SessionManager();

    handlers = new()
    {
      {typeof(PlayerStartMovement), OnPlayerStartMovement},
      {typeof(PlayerStopMovement), OnPlayerStopMovement},
      {typeof(PlayerRequestSkill), OnPlayerRequestSkill},
      {typeof(PlayerUseItem), OnPlayerUseItem},
      {typeof(PlayerPickUpItem), OnPlayerPickUpItem},
      {typeof(EnterSessionMap), OnEnterSessionMap},
      {typeof(FetchServerTime), OnFetchServerTime},
      {typeof(CMAudioVoiceData), OnCMAudioVoiceData},
      {typeof(CMQuestRequestList), OnQuestListRequested},
      {typeof(CMQuestJoin), OnQuestJoin},
    };
  }

  public void OnPacketHandler(long id, byte[] pck)
  {
    try
    {
      var command = MessagePackSerializer.Deserialize<IClientCommand>(pck);

      Player session = sessionManager.GetActor((int)id);

      handlers[command.GetType()].Invoke(id, session, command);
    }
    catch (Exception e)
    {
      GD.Print(string.Format("Peer: {0} - {1}", id, e.Message));
      GD.Print(e.StackTrace);

      Networking.Instance.Disconnect((int)id);
    }
  }
}
