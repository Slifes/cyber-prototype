using Godot;


class UIControl
{
  HealthStats healthStats;

  SkillList skillList;

  SkillShortcut skillShortcut;

  private static UIControl _instance;

  public static UIControl Instance
  {
    get { return _instance; }
  }

  public static void CreateInstance()
  {
    _instance = new UIControl();
  }

  public SkillItem GetSkillSlot(int index)
  {
    return skillShortcut.GetSkillBySlot(index);
  }

  public void LoadUI(Player target)
  {
    var uiScene = ResourceLoader.Load<PackedScene>("res://ui/master.tscn");

    var ui = uiScene.Instantiate<Control>();

    target.GetNode("/root/World/UI").AddChild(ui);

    healthStats = ui.GetNode<HealthStats>("HealthStats");
    skillList = ui.GetNode<SkillList>("MarginContainer/SkillList");
    skillShortcut = ui.GetNode<SkillShortcut>("MarginContainer2/SkillShortcut");

    target.HealthStatusChanged += UpdateHealthStats;

    skillList.SetSkills(target.Skills);
  }

  void UpdateHealthStats(int currentHP, int currentSP, int maxHP, int maxSP)
  {
    healthStats.SetCurrentHP(currentHP, maxHP);
  }
}