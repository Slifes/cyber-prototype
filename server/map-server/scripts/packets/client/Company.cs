using MessagePack;

namespace Packets.Client
{
  [MessagePackObject]
  public partial struct CMQuestOpenList : IClientCommand
  {
    [Key(0)] public int CompanyID;
  }

  [MessagePackObject]
  public partial struct CMQuestJoin : IClientCommand
  {
    [Key(0)] public int QuestID;
  }

  [MessagePackObject]
  public partial struct CMQuestOpenDetail : IClientCommand
  {
    [Key(0)] public int QuestID;
  }

  [MessagePackObject]
  public partial struct CMQuestLeave : IClientCommand
  {
    [Key(0)] public int QuestID;
  }
}
