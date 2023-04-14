using System.Collections.Generic;

class QuestManager
{
  static QuestManager _instance;

  public static QuestManager Instance { get { return _instance; } }

  Dictionary<int, List<QuestDetail>> quests = new();

  public QuestManager()
  {
    _instance = this;

    quests.Add(1, new List<QuestDetail>() {
      new QuestDetail()
      {
        ID = 1,
        ActorID = 1,
        DescriptionID = 0,
        Reputation = 1,
        TimeLimit = 1,
        QuestRequiredID = 1,
        Target = new QuestTarget()
        {
          Action = QuestAction.EnemyKilled,
          ReferenceID = 0,
          Amount = 5,
        },
        Rewards = new QuestReward[]
        {
          new QuestReward()
          {
            Type = QuestRewardType.Money,
            Value = 10,
          },
          new QuestReward()
          {
            Type = QuestRewardType.Item,
            Value = 1,
          },
          new QuestReward()
          {
            Type = QuestRewardType.Reputation,
            Value = 1,
          },
        },
      }
    });
  }

  public List<QuestDetail> GetQuestList(int boardId)
  {
    return quests[boardId];
  }
}
