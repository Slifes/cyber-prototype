namespace Packets.Server
{

  [MessagePack.Union(0, typeof(SMActorEnteredZone))]
  [MessagePack.Union(1, typeof(SMActorExitedZone))]
  [MessagePack.Union(2, typeof(SMExecuteSkill))]
  [MessagePack.Union(3, typeof(PlayerEntered))]
  [MessagePack.Union(4, typeof(InvetoryList))]
  [MessagePack.Union(5, typeof(SkillList))]
  [MessagePack.Union(6, typeof(ServerTime))]
  [MessagePack.Union(7, typeof(SMActorStartMove))]
  [MessagePack.Union(8, typeof(SMActorStopMove))]
  [MessagePack.Union(9, typeof(SMActorDamage))]
  public interface IServerCommand { }
}
