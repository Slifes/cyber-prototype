using Godot;

partial class MiniHealth : IComponent
{
  static PackedScene healthScene = ResourceLoader.Load<PackedScene>("res://components/Health.tscn");

  ProgressBar hpBar;

  Sprite3D HP;

  public MiniHealth(CharacterActor actor)
  {
    var node = healthScene.Instantiate();

    actor.AddChild(node);

    HP = node.GetNode<Sprite3D>("HP");

    hpBar = node.GetNode<ProgressBar>("SubViewport/ProgressBar");

    actor.HealthStatusChanged += HealthChanged;
    actor.TakeDamage += TakeDamage;
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
