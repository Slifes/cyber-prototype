using Godot;

enum EffectType
{
  Heal,
  Damage,
}

class EffectComponent : IComponent
{
  static PackedScene effect = ResourceLoader.Load<PackedScene>("res://effects/Damage.tscn");

  CharacterActor actor;

  public EffectComponent(CharacterActor actor)
  {
    this.actor = actor;

    this.actor.Effect += OnEffect;
  }

  void OnEffect(int effectType, int value)
  {
    var instance = effect.Instantiate<Node3D>();

    actor.AddChild(instance);

    switch ((EffectType)effectType)
    {
      case EffectType.Heal:
        instance.Call("run", value, "#00ff00");
        break;

      case EffectType.Damage:
        var color = actor.IsMultiplayerAuthority() ? "#ff0002" : "#ffffff";
        instance.Call("run", value, color);
        break;
    }
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
