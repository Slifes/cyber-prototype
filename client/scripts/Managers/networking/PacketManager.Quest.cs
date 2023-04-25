using Packets.Server;
using System.Linq;

partial class PacketManager
{
  void OnQuestList(IServerCommand command)
  {
    var questList = (SMQuestList)command;

    // QuestManager.Instance.OpenQuestList(questList.Quests);

    QuestManager.Instance.OpenQuestList(questList.Quests.Select(quest => new Quest
    {
      ID = quest.ID,
      Title = quest.DescriptionID.ToString(),
      Description = quest.DescriptionID.ToString(),
      Reputation = quest.Reputation,
      Rewards = quest.Rewards.Select(reward => new QuestReward
      {
        Type = reward.Type,
        Value = reward.Value
      }).ToList(),
      Target = new QuestTarget
      {
        Action = quest.Target.Action,
        ReferenceID = quest.Target.ReferenceID,
        Amount = quest.Target.Amount
      }
    }).ToArray());
  }
}
