using MessagePack;

namespace Packets.Server
{
  [MessagePackObject]
  public partial struct PlayerEntered : IServerCommand
  {

  }

  [MessagePackObject]
  public partial struct SkillList : IServerCommand
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
  public partial struct InvetoryList : IServerCommand
  {
    [Key(0)] public PckItem[] items;
  }

  [MessagePackObject]
  public partial struct ServerTime : IServerCommand
  {
    [Key(0)] public ulong Time;
    [Key(1)] public double ClientTime;
  }
}
