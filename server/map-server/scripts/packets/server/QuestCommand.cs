using MessagePack;

namespace Packets.Server
{
  [MessagePackObject]
  public partial struct SMQuestItem
  {
    [Key(0)] public int ID;
    [Key(1)] public int DescriptionID;
    [Key(2)] public int Reputation;
    [Key(3)] public QuestReward[] Rewards;
    [Key(4)] public QuestTarget Target;
  }

  [MessagePackObject]
  public partial struct SMQuestList : IServerCommand
  {
    [Key(0)] public int CompanyID;
    [Key(1)] public SMQuestItem[] Quests;
  }

  [MessagePackObject]
  public partial struct SMQuestUpdateProgress : IServerCommand
  {
    [Key(0)] public int QuestID;
    [Key(1)] public int Progress;
  }

  [MessagePackObject]
  public partial struct SMQuestPlayerJoined : IServerCommand
  {
    [Key(0)] public int QuestID;
    [Key(1)] public int Progress;
  }

  [MessagePackObject]
  public partial struct SMQuestPlayerLeft : IServerCommand
  {
    [Key(0)] public int QuestID;
  }

  [MessagePackObject]
  public partial struct SMQuestCompleted : IServerCommand
  {
    [Key(0)] public int QuestID;
    [Key(1)] public QuestReward[] Rewards;
  }

  [MessagePackObject]
  public partial struct SMQuestPlayerList : IServerCommand
  {
    [Key(0)] public int[] QuestIDs;
  }
}
