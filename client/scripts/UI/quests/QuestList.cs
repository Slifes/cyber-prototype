using Godot;

partial class QuestList : Window
{
  static PackedScene ItemScene = ResourceLoader.Load<PackedScene>("res://ui/quests/quest_item.tscn");

  Quest[] quests;

  VBoxContainer container;

  public override void _Ready()
  {
    container = GetNode<VBoxContainer>("MarginContainer/VBoxContainer/ScrollContainer/VBoxContainer");
  }

  public void SetQuests(Quest[] quests)
  {
    this.quests = quests;

    foreach (var quest in quests)
    {
      var item = ItemScene.Instantiate<QuestItem>();

      item.Quest = quest;

      container.AddChild(item);
    }
  }
}
