using Packets.Client;

partial class PacketManager
{
  public void OnQuestListRequested(long peerId, Player actor, IClientCommand command)
  {
    var pck = (CMQuestRequestList)command;

    var quests = QuestManager.Instance.GetQuestList(pck.BoardID);

    // Networking.Instance.SendPacket(peerId, new SMQuestList
    // {
    //   Quests = QuestManager.
    // });
  }
}
