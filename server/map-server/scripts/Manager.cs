using Godot;

partial class Manager : Node
{
  public override void _Ready()
  {
    ItemManager.CreateInstance("items");
    ItemManager.Instance.Load();

    SkillManager.CreateInstance("skills");
    SkillManager.Instance.Load();
  }
}
