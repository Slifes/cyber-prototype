using Packets.Client;

partial class PacketManager
{
  public void OnQuestListRequested(long peerId, Player actor, IClientCommand command)
  {
    var pck = (CMQuestRequestList)command;

    var quests = QuestManager.Instance.GetQuestList(pck.BoardID);

    var pckQuests = new Packets.Server.SMQuestItem[quests.Count];

    for (var i = 0; i < quests.Count; i++)
    {
      pckQuests[i] = new Packets.Server.SMQuestItem
      {
        ID = quests[i].ID,
        DescriptionID = quests[i].DescriptionID,
        Reputation = quests[i].Reputation,
        Rewards = quests[i].Rewards,
        Target = quests[i].Target,
      };
    }

    Networking.Instance.SendPacket(peerId, new Packets.Server.SMQuestList
    {
      CompanyID = pck.BoardID,
      Quests = pckQuests
    });
  }

  public void OnQuestJoin(long peerId, Player actor, IClientCommand command)
  {
    var pck = (CMQuestJoin)command;

    // var quest = QuestManager.Instance.GetQuest(pck.QuestID);

    // if (quest == null)
    // {
    //   return;
    // }

    // quest.Join();
  }
}
