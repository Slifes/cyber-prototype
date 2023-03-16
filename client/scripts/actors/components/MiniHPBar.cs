using Godot;

class MiniHPBar : IComponent
{
  static PackedScene healthScene = ResourceLoader.Load<PackedScene>("res://components/health.tscn");

  ProgressBar hpBar;

  SubViewport view;

  Sprite3D HP;

  public MiniHPBar(CharacterActor actor)
  {
    var node = healthScene.Instantiate();

    actor.AddChild(node);

    HP = node.GetNode<Sprite3D>("HP");

    view = node.GetNode<SubViewport>("SubViewport");

    hpBar = node.GetNode<ProgressBar>("SubViewport/ProgressBar");

    actor.HealthStatusChanged += HealthChanged;
    actor.TakeDamage += TakeDamage;

    // var texture = new ViewportTexture();
    // texture.ViewportPath = HP.GetPathTo(view);

    // HP.Texture = texture;
  }

  void HealthChanged(int currentHP, int maxHP, int currentSP, int maxSP)
  {
    hpBar.Value = currentHP;
    hpBar.MaxValue = maxHP;
  }

  void TakeDamage(int damage, int currentHP, int maxHP)
  {
    GD.Print(hpBar.Value);

    hpBar.Value = currentHP;
    HP.Visible = true;

    GD.Print("Took Damage");
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
