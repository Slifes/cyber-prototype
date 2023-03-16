using Godot;
using System.Collections.Generic;


class UIControl
{
  HealthStats healthStats;

  SkillList skillList;

  SkillShortcut skillShortcut;

  UIEquipment equipment;

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

    healthStats = ui.GetNode<HealthStats>("Health");
    skillList = ui.GetNode<SkillList>("SkillBook");
    skillShortcut = ui.GetNode<SkillShortcut>("Shortcuts/SkillShortcut");
    equipment = ui.GetNode<UIEquipment>("Equipment");

    equipment.setPlayer(target);

    target.HealthStatusChanged += UpdateHealthStats;

    UpdateHP(target.GetCurrentHP(), target.GetMaxHP());
  }

  void UpdateHealthStats(int currentHP, int currentSP, int maxHP, int maxSP)
  {
    healthStats.SetCurrentHP(currentHP, maxHP);
  }

  public void UpdateHP(int currentHP, int maxHP)
  {
    healthStats.SetCurrentHP(currentHP, maxHP);
  }

  public static void SetSkills(List<Skill> skills)
  {
    _instance.skillList.SetSkills(skills);
  }
}
