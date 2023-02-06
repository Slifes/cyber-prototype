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
  }

  void ExecutedSkill(Variant id)
  {
    SkillControl.Instance.UpdateSkillItems((int)id, 0);
  }

  void UpdateHealth(int currentHP, int maxHP, int currentSP, int maxSP)
  {

  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta)
  {
    for (var i = 0; i < 6; i++)
    {
      if (Input.IsActionJustPressed(String.Format("slot{0}", i)))
      {
        var skillItem = UIControl.Instance.GetSkillSlot(i);

        if (skillItem != null && skillItem.Available)
        {
          actor.SendRequestSkill(skillItem.skill.ID, new Variant());
        }
      }
    }
  }
}
