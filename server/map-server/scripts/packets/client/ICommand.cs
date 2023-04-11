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
  public interface IClientCommand { }
}
