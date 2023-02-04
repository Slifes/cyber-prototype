using Godot;
using System;

class UIComponent : IComponent
{
  Player actor;

  public UIComponent(Player player)
  {
    this.actor = player;

    this.actor.ExecuteSkill += ExecutedSkill;

    UIControl.CreateInstance();
    UIControl.Instance.LoadUI(player);
  }

  void ExecutedSkill(Variant id)
  {
    SkillControl.Instance.UpdateSkillItems((int)id, 0);
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
