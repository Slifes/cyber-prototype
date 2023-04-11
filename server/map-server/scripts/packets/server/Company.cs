using MessagePack;
using System.Collections.Generic;

namespace Packets.Server
{
  [MessagePackObject]
  public partial struct SMQuestDetail : IServerCommand
  {
    [Key(0)] public int ID;
    [Key(1)] public int Name;
    [Key(3)] public int ReputationRequired;
    [Key(4)] public Dictionary<RewardType, int> Rewards;
  }

  [MessagePackObject]
  public partial struct SMQuestItem
  {
    [Key(0)] public int ID;
    [Key(1)] public int Name;
  }

  [MessagePackObject]
  public partial struct SMQuestStepDetail : IServerCommand
  {
    [Key(1)] public int QuestID;
    [Key(2)] public int StepIndex;
    [Key(3)] public int StepType;
  }

  [MessagePackObject]
  public partial struct SMQuestList : IServerCommand
  {
    [Key(0)] public int CompanyID;
    [Key(1)] public SMQuestItem[] Quests;
  }

  [MessagePackObject]
  public partial struct SMQuestStatus : IServerCommand
  {
    [Key(0)] public int ID;
    [Key(1)] public int CurrentStep;
  }

  [MessagePackObject]
  public partial struct SMQuestPlayerJoined : IServerCommand
  {
    [Key(0)] public int QuestID;
    [Key(1)] public int PlayerID;
  }
}
