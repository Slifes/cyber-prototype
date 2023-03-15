namespace Packets.Server
{
  [MessagePack.Union(1, typeof(SMServerTime))]
  [MessagePack.Union(10, typeof(SMActorEnteredZone))]
  [MessagePack.Union(11, typeof(SMActorExitedZone))]
  [MessagePack.Union(20, typeof(SMInvetoryList))]
  [MessagePack.Union(21, typeof(SMInventoryAddItem))]
  [MessagePack.Union(22, typeof(SMInventoryRemoveItem))]
  [MessagePack.Union(23, typeof(SMEquipmentList))]
  [MessagePack.Union(24, typeof(SMEquipmentApplied))]
  [MessagePack.Union(25, typeof(SMEquipmentRemoved))]
  [MessagePack.Union(26, typeof(SMSkillList))]
  [MessagePack.Union(30, typeof(SMActorStartMove))]
  [MessagePack.Union(31, typeof(SMActorStopMove))]
  [MessagePack.Union(32, typeof(SMActorDamage))]
  [MessagePack.Union(33, typeof(SMActorExecuteSkill))]
  public interface IServerCommand { }
}
