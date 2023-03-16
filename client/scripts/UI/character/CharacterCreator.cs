using Godot;

partial class CharacterCreator : Node3D
{
  AuthClient authClient;

  Button CreateBtn;

  TextEdit Input;

  ColorPickerButton ColorPicker;

  MeshInstance3D PlayerMesh;

  StandardMaterial3D Material;

  Color color;

  public override void _Ready()
  {
    authClient = GetNode<AuthClient>("/root/AuthClient");
    CreateBtn = GetNode<Button>("Control2/Name/MarginContainer/VBoxContainer/Button");
    Input = GetNode<TextEdit>("Control2/Name/MarginContainer/VBoxContainer/TextEdit");
    ColorPicker = GetNode<ColorPickerButton>("Control2/Color/Panel/MarginContainer/VBoxContainer/ColorPickerButton");

    PlayerMesh = GetNode<MeshInstance3D>("Player/Body/MeshInstance3D");

    Material = (StandardMaterial3D)PlayerMesh.Mesh.SurfaceGetMaterial(0);

    ColorPicker.ColorChanged += OnColorChanged;

    ColorPicker.Color = Material.AlbedoColor;

    CreateBtn.Pressed += OnSubmit;
  }

  void OnColorChanged(Color color)
  {
    this.color = color;

    Material.AlbedoColor = color;
  }

  void Validate()
  {
    string name = Input.Text;
    string color = this.color.ToString();
  }

  async void OnSubmit()
  {
    string name = Input.Text;
    string color = this.color.ToHtml();

    var data = await authClient.SendCreateCharacter(authClient.TokenSelected, name, color);

    GetNode<SceneManager>("/root/SceneManager").ChangeState("authenticate");
  }
}
