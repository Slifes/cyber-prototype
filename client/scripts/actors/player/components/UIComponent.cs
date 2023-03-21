using Godot;
using System;

class UIComponent : IComponent
{
  Player actor;

  public UIComponent(Player player)
  {
    PlayerUI.Instance.LoadPlayerToUI(player);

    this.actor = player;

    this.actor.ExecuteSkill += ExecutedSkill;
    this.actor.HealthStatusChanged += UpdateHealth;
    // this.actor.Effect += TakeDamage;
  }

  void ExecutedSkill(Variant id)
  {
    SkillControl.Instance.UpdateSkillItems((int)id, 0);
  }

  void UpdateHealth(int currentHP, int maxHP, int currentSP, int maxSP)
  {
    PlayerUI.Instance.UpdateHP(currentHP, maxHP);
  }

  void TakeDamage(int damage, int currentHP, int maxHP)
  {
    PlayerUI.Instance.UpdateHP(currentHP, maxHP);
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
