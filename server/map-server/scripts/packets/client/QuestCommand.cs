using MessagePack;

namespace Packets.Client
{
  [MessagePackObject]
  public partial struct CMQuestRequestList : IClientCommand
  {
    [Key(0)] public int BoardID;
  }

  [MessagePackObject]
  public partial struct CMQuestJoin : IClientCommand
  {
    [Key(0)] public int QuestID;
  }

  [MessagePackObject]
  public partial struct CMQuestLeave : IClientCommand
  {
    [Key(0)] public int QuestID;
  }
}
