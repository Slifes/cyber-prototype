using Godot;

partial class SkillItem: Control
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

  public override void _Ready()
  {
    Overlay = GetNode<ColorRect>("View/overlay");

    GetNode<ColorRect>("View/bkg").Color = skill.iconColor;
  }

  public override void _ExitTree()
  {
    SkillControl.Instance.Remove(this);
  }

  public void Used()
  {
    Available = false;
    TimeLapsed = 0;

    Overlay.Position = new Vector2(Overlay.Position.x, 0);
    Overlay.Size = new Vector2(Overlay.Size.x, this.Size.y);
  }

  public override void _Process(double delta)
  {
    if (!Available)
    {
      TimeLapsed += (float)delta;

      if (TimeLapsed >= skill.Delay)
      {
        Available = true;
      }
      else
      {
        var percentage = (float)TimeLapsed / skill.Delay;
        var size = this.Size.y * percentage;

        Overlay.Position = new Vector2(Overlay.Position.x, size);
        Overlay.Size = new Vector2(Overlay.Size.x, this.Size.y - size);
      }
    }
  }
}
