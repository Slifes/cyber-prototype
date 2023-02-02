using Godot;
using System;

class UIComponent : IPlayerComponent
{
  Player actor;

  public UIComponent(Player player)
  {
    this.actor = player;

    UIControl.CreateInstance();
    UIControl.Instance.LoadUI(player);
  }

  public void InputHandler(InputEvent @event)
  {

  }

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
