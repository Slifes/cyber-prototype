using Godot;

partial class QuestRewardItem : Control
{
  ColorRect colorRect;

  public QuestReward Reward;

  public override void _Ready()
  {
    base._Ready();

    colorRect = GetNode<ColorRect>("ColorRect");

    SetReward(Reward);
  }

  public void SetReward(QuestReward reward)
  {
    switch (reward.Type)
    {
      case QuestRewardType.Item:
        var item = ItemManager.Instance.GetItem(reward.Value);
        colorRect.Color = item.Icon;
        break;
      case QuestRewardType.Money:
        colorRect.Color = new Color(255, 84.3f, 0);
        break;
      case QuestRewardType.Reputation:
        colorRect.Color = new Color(0, 0, 255);
        break;
    }
  }
}
