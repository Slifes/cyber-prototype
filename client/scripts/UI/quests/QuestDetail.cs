using Godot;

partial class Quest : RefCounted
{
  public int ID;
  public string Title;
  public string Description;
  public int Reputation;
}

partial class QuestDetail : Window
{
  [Export]
  public Quest Quest;

  Label Title;
  Label Description;
  Label Reputation;

  Button Join;
  Button Cancel;

  public override void _Ready()
  {
    Title = GetNode<Label>("MarginContainer/VBoxContainer/Title/Value");
    Reputation = GetNode<Label>("MarginContainer/VBoxContainer/Reputation/Value");
    Description = GetNode<Label>("MarginContainer/VBoxContainer/Description/Value");

    Join = GetNode<Button>("MarginContainer/HBoxContainer/Join");
    Cancel = GetNode<Button>("MarginContainer/HBoxContainer/Leave");

    Join.Pressed += OnJoinPressed;
    Cancel.Pressed += OnCancelPressed;
  }

  void OnJoinPressed()
  {
    GD.Print("Joined");
  }

  void OnCancelPressed()
  {
    GD.Print("Canceled");
  }
}
