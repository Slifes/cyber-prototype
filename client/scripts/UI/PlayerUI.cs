using Godot;
using System.Collections.Generic;


partial class PlayerUI : Control
{
  HealthStats healthStats;

  SkillList skillList;

  Shortcut shortcut;

  public Inventory inventory;

  UIEquipment equipment;

  private static PlayerUI _instance;

  public static PlayerUI Instance
  {
    get { return _instance; }
  }

  public override void _Ready()
  {
    _instance = this;

    healthStats = GetNode<HealthStats>("Health");
    skillList = GetNode<SkillList>("SkillBook");
    shortcut = GetNode<Shortcut>("Shortcuts/SkillShortcut");
    equipment = GetNode<UIEquipment>("Equipment");
    inventory = GetNode<Inventory>("Inventory");
  }

  public void LoadPlayerToUI(Player target)
  {
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

  // public static void SetInventoryList(List<)
}
