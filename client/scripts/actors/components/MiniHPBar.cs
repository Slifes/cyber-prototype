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
    actor.Effect += OnEffect;

    HealthChanged(actor.GetCurrentHP(), actor.GetMaxHP(), actor.GetCurrentSP(), actor.GetMaxSP());
  }

  void HealthChanged(int currentHP, int maxHP, int currentSP, int maxSP)
  {
    hpBar.Value = currentHP;
    hpBar.MaxValue = maxHP;
  }

  void OnEffect(int effectType, int effectValue)
  {
    switch ((EffectType)effectType)
    {
      case EffectType.Damage:
        hpBar.Value -= effectValue;
        break;

      case EffectType.Heal:
        hpBar.Value += effectValue;
        break;
    }

    HP.Visible = true;
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
