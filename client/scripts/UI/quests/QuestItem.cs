using Godot;

partial class QuestItem : Button
{
  Label Title;

  [Export]
  public Quest Quest;

  public override void _Ready()
  {
    Title = GetNode<Label>("MarginContainer/Title");

    Pressed += OnPressed;

    Title.Text = Quest.Title;
  }

  void OnPressed()
  {
    QuestManager.Instance.OpenQuestDetail(Quest);
  }
}
