using MessagePack;

namespace Packets.Server
{
  [MessagePackObject]
  public partial struct SMSkillList : IServerCommand
  {
    [Key(0)] public int[] skillsId;
  }

  [MessagePackObject]
  public partial struct PckItem : IServerCommand
  {
    [Key(0)] public int id;
    [Key(1)] public int amount;
  }

  [MessagePackObject]
  public partial struct SMInvetoryList : IServerCommand
  {
    [Key(0)] public PckItem[] items;
  }

  [MessagePackObject]
  public partial struct SMInventoryAddItem : IServerCommand
  {
    [Key(0)] public int itemId;
    [Key(1)] public int amount;
  }

  [MessagePackObject]
  public partial struct SMInventoryRemoveItem : IServerCommand
  {
    [Key(0)] public int itemId;
    [Key(1)] public int amount;
  }

  [MessagePackObject]
  public partial struct SMEquipmentList : IServerCommand
  {
    [Key(0)] public System.Collections.Generic.Dictionary<EquipmentSlot, int> equips;
  }

  [MessagePackObject]
  public partial struct SMEquipmentApplied : IServerCommand
  {
    [Key(0)] public int ActorId;
    [Key(1)] public int equipmenetId;
    [Key(2)] public EquipmentSlot slot;
  }

  [MessagePackObject]
  public partial struct SMEquipmentRemoved : IServerCommand
  {
    [Key(0)] public int ActorId;
    [Key(1)] public EquipmentSlot slot;
  }

  [MessagePackObject]
  public partial struct SMServerTime : IServerCommand
  {
    [Key(0)] public ulong Time;
    [Key(1)] public double ClientTime;
  }
}
