using Godot;
using System;

class UIComponent : IComponent
{
  Player actor;

  public UIComponent(Player player)
  {
    UIControl.CreateInstance();
    UIControl.Instance.LoadUI(player);

    this.actor = player;

    this.actor.ExecuteSkill += ExecutedSkill;
    this.actor.HealthStatusChanged += UpdateHealth;
    this.actor.TakeDamage += TakeDamage;
  }

  void ExecutedSkill(Variant id)
  {
    SkillControl.Instance.UpdateSkillItems((int)id, 0);
  }

  void UpdateHealth(int currentHP, int maxHP, int currentSP, int maxSP)
  {
    UIControl.Instance.UpdateHP(currentHP, maxHP);
  }

  void TakeDamage(int damage, int currentHP, int maxHP)
  {
    UIControl.Instance.UpdateHP(currentHP, maxHP);
  }

  public void InputHandler(InputEvent @event) { }

  void SendSkillBySlot(int index)
  {
    var skillItem = UIControl.Instance.GetSkillSlot(index);

    if (skillItem != null && skillItem.Available)
    {
      actor.SendRequestSkill(skillItem.skill.ID, new Variant());
    }
  }

  public void Update(float delta)
  {
    if (Input.IsActionJustPressed("attack"))
    {
      SendSkillBySlot(0);
    }

    for (var i = 0; i < 6; i++)
    {
      if (Input.IsActionJustPressed(String.Format("slot{0}", i)))
      {
        SendSkillBySlot(i);
      }
    }
  }
}
