using Godot;

partial class UIEquipment : Window
{
  UICameraEquip camera;

  public override void _Ready()
  {
    base._Ready();

    // camera = GetNode<UICameraEquip>("SubViewport/CameraPivot");
  }

  public void setPlayer(Player player)
  {
    // camera.player = player;
  }
}
