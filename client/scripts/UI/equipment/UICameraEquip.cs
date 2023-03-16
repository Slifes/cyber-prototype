using Godot;

partial class UICameraEquip : Node3D
{
  public Player player { get; set; }

  public override void _Process(double delta)
  {
    if (player != null)
    {
      Position = player.Position;
      Rotation = player.GetBodyRotation();
    }
  }
}

