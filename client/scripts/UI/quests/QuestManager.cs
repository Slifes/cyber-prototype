using Godot;
using System.Collections.Generic;
using Packets.Client;
using MessagePack;

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

partial class Quest : RefCounted
{
  public int ID;
  public string Title;
  public string Description;
  public int Reputation;
  public List<QuestReward> Rewards;
  public QuestTarget Target;
}

partial class QuestManager : Control
{
  QuestDetail questDetail;

  QuestList questList;

  static QuestManager _instance;

  public static QuestManager Instance { get { return _instance; } }

  public override void _Ready()
  {
    _instance = this;

    questDetail = GetNode<QuestDetail>("QuestDetail");
    questList = GetNode<QuestList>("QuestList");

    GetNode<Node>("/root/Console").Call("register_env", "quest_manager", this);
  }

  public void GetQuestList(int boardId)
  {
    NetworkManager.Instance.SendPacket(new CMQuestRequestList { BoardID = boardId });
  }

  public void OpenQuestDetail(Quest quest)
  {
    questDetail.OpenQuest(quest);
  }

  public void OpenQuestList(Quest[] quests)
  {
    questList.OpenQuestList(quests);
  }
}
