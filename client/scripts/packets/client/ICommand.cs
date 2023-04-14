using MessagePack;

namespace Packets.Client
{
  [Union(0, typeof(EnterSessionMap))]
  [Union(1, typeof(FetchServerTime))]
  [Union(2, typeof(PlayerStartMovement))]
  [Union(3, typeof(PlayerStopMovement))]
  [Union(4, typeof(PlayerRequestSkill))]
  [Union(5, typeof(PlayerUseItem))]
  [Union(6, typeof(PlayerPickUpItem))]
  [Union(7, typeof(CMAudioVoiceData))]
  [Union(20, typeof(CMQuestRequestList))]
  [Union(21, typeof(CMQuestJoin))]
  [Union(22, typeof(CMQuestLeave))]
  public interface IClientCommand { }
}
