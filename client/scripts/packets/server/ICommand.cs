namespace Packets.Server
{

  [MessagePack.Union(0, typeof(SMActorEnteredZone))]
  [MessagePack.Union(1, typeof(SMActorExitedZone))]
  [MessagePack.Union(2, typeof(PlayerEntered))]
  [MessagePack.Union(3, typeof(InvetoryList))]
  [MessagePack.Union(4, typeof(SkillList))]
  [MessagePack.Union(5, typeof(ServerTime))]
  public interface IServerCommand { }
}
