using Godot;

partial class Shortcut : Control
{
  ShortcutSlot[] slots;

  IUsable[] usables;

  public override void _Ready()
  {
    var hBox = GetNode<Control>("ColorRect/MarginContainer/HBoxContainer");

    slots = new ShortcutSlot[hBox.GetChildCount()];

    usables = new IUsable[hBox.GetChildCount()];

    for (var i = 0; i < slots.Length; i++)
    {
      slots[i] = (ShortcutSlot)hBox.GetChild(i);
      slots[i].Changed += (Node node, int index) => { usables[index] = (IUsable)node; };
    }
  }

  public IUsable GetUsable(int index)
  {
    return usables[index];
  }

  void Execute(int index)
  {
    var item = usables[index];

    if (item != null && item.IsAvailable())
    {
      item.Use();
    }
  }

  public override void _Process(double delta)
  {
    if (Input.IsActionJustPressed("attack"))
    {
      Execute(0);
    }

    for (var i = 0; i < 6; i++)
    {
      if (Input.IsActionJustPressed(string.Format("slot{0}", i)))
      {
        Execute(i);
      }
    }
  }
}
