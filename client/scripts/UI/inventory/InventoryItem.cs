using Godot;

partial class InventoryItem : Control
{
  [Export]
  public Item item;

  [Export]
  public int Amount
  {
    set
    {
      amount = value;

      if (label != null)
      {
        label.Text = value.ToString();
      }
    }

    get { return amount; }
  }

  Label label;

  ColorRect texture;

  private int amount;

  private Color bkg;

  public override void _Ready()
  {
    label = GetNode<Label>("View/Label");
    texture = GetNode<ColorRect>("View/bkg");

    label.Text = amount.ToString();
    texture.Color = item.Icon;

    //ItemControl.Instance.Add(this);
  }

  public override void _ExitTree()
  {
    // ItemControl.Instance.Remove(this);
  }
}
