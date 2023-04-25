using Godot;
using MessagePack;
using System.Collections.Generic;

public enum QuestRewardType
{
  Money,
  Item,
  Reputation,
}

public enum QuestAction
{
  ItemCollected,
  EnemyKilled,
  NpcTalked,
  ChallengeCompleted,
}

public enum QuestStatus
{
  Accepted,
  Completed,
  Failed,
}

[MessagePackObject]
public struct QuestReward
{
  [Key(0)] public QuestRewardType Type;
  [Key(1)] public int Value;
}

[MessagePackObject]
public struct QuestTarget
{
  [Key(0)] public QuestAction Action;
  [Key(1)] public int ReferenceID;
  [Key(2)] public int Amount;
}

class QuestDetail
{
  public int ID;
  public int ActorID;
  public int DescriptionID;
  public int Reputation;
  public int TimeLimit;
  public int QuestRequiredID;
  public QuestTarget Target;
  public QuestReward[] Rewards;
  public Dictionary<int, QuestProgress> Progress = new();
}

class QuestProgress
{
  public Player actor;
  public QuestDetail Quest;
  public QuestStatus Status;
  public int Progress;

  public void UpdateQuestByTarget(QuestAction action, int refId, int amount)
  {
    if (action == Quest.Target.Action && refId == Quest.Target.ReferenceID)
    {
      Progress += amount;

      if (Progress >= Quest.Target.Amount)
      {
        Status = QuestStatus.Completed;
        CollectReward();
      }
    }
  }

  public void CollectReward()
  {
    foreach (var reward in Quest.Rewards)
    {
      switch (reward.Type)
      {
        case QuestRewardType.Money:
          actor.Zeny.Receive(reward.Value);
          break;
        case QuestRewardType.Item:
          actor.Inv.Add(reward.Value, 1);
          break;
        case QuestRewardType.Reputation:
          break;
      }
    }
  }
}
