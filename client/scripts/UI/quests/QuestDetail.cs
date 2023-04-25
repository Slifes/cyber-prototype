using Godot;
using Packets.Client;

partial class QuestDetail : Window
{
  static PackedScene RewardItemScene = GD.Load<PackedScene>("res://ui/quests/quest_reward_item.tscn");

  public Quest Quest;

  Label Title;
  Label Description;
  Label Reputation;
  Button Join;
  Button Cancel;
  HBoxContainer RewardContainer;

  public override void _Ready()
  {
    Title = GetNode<Label>("MarginContainer/VBoxContainer/Title/Value");
    Reputation = GetNode<Label>("MarginContainer/VBoxContainer/Reputation/Value");
    Description = GetNode<Label>("MarginContainer/VBoxContainer/Description/Value");

    Join = GetNode<Button>("MarginContainer/VBoxContainer2/HBoxContainer/Join");
    Cancel = GetNode<Button>("MarginContainer/VBoxContainer2/HBoxContainer/Leave");

    RewardContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer2/Reward/Rewards");

    Join.Pressed += OnJoinPressed;
    Cancel.Pressed += OnCancelPressed;
  }

  public void OpenQuest(Quest quest)
  {
    GD.Print("Opening quest: " + quest.ID);
    var detail = GD.Load<QuestDescription>("res://resources/quests/" + quest.ID + ".tres");

    Title.Text = quest.Title;
    Description.Text = quest.Description;
    Reputation.Text = quest.Reputation.ToString();

    ClearReward();

    quest.Rewards.ForEach(CreateReward);

    Visible = true;
  }

  void ClearReward()
  {
    for (var i = 0; i < RewardContainer.GetChildCount(); i++)
    {
      RewardContainer.GetChild(i).QueueFree();
    }
  }

  void CreateReward(QuestReward reward)
  {
    var rewardItem = RewardItemScene.Instantiate<QuestRewardItem>();

    rewardItem.Reward = reward;

    RewardContainer.AddChild(rewardItem);
  }

  void OnJoinPressed()
  {
    GD.Print("Joined");

    NetworkManager.Instance.SendPacket(new CMQuestJoin { QuestID = Quest.ID });
  }

  void OnCancelPressed()
  {
    GD.Print("Canceled");
    Visible = false;
  }
}
