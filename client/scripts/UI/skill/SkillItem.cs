﻿using Godot;

partial class SkillItem : Control, IUsable
{
  ColorRect Overlay;

  [Export]
  public Skill skill;

  [Export]
  public float TimeLapsed;

  [Export]
  public bool Available { get; set; } = true;

  public override void _EnterTree()
  {
    SkillControl.Instance.Add(this);
  }

  public override void _ExitTree()
  {
    SkillControl.Instance.Remove(this);
  }

  public override void _Ready()
  {
    Overlay = GetNode<ColorRect>("View/overlay");

    GetNode<ColorRect>("View/bkg").Color = skill.iconColor;
  }

  public void Used()
  {
    Available = false;
    TimeLapsed = 0;

    Overlay.Position = new Vector2(Overlay.Position.X, 0);
    Overlay.Size = new Vector2(Overlay.Size.X, this.Size.Y);
  }

  public void Use()
  {
    NetworkManager.Instance.SendPacket(new Packets.Client.PlayerRequestSkill
    {
      skillId = skill.ID
    });
  }

  public override void _Process(double delta)
  {
    if (!Available)
    {
      TimeLapsed += (float)delta;

      if (TimeLapsed >= skill.Delay)
      {
        Available = true;
        Overlay.Position = new Vector2(Overlay.Position.X, this.Size.Y);
        Overlay.Size = new Vector2(Overlay.Size.X, 0);
      }
      else
      {
        var percentage = (float)TimeLapsed / skill.Delay;
        var size = this.Size.Y * percentage;

        Overlay.Position = new Vector2(Overlay.Position.X, size);
        Overlay.Size = new Vector2(Overlay.Size.X, this.Size.Y - size);
      }
    }
  }

  public Node GetData()
  {
    return this;
  }

  public bool IsAvailable()
  {
    return Available;
  }
}
