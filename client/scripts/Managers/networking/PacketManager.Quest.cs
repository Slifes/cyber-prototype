using Packets.Server;

partial class PacketManager
{
  void OnQuestList(IServerCommand command)
  {
    var questList = (SMQuestList)command;

    // QuestManager.Instance.OpenQuestList(questList.Quests);
  }
}
