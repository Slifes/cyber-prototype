using Godot;

class DamageLabel : IComponent
{
  CharacterActor actor;

  public DamageLabel(CharacterActor actor)
  {
    this.actor = actor;

    this.actor.TakeDamage += TakeDamage;
  }

  void TakeDamage(int damage, int currentHP, int maxHP)
  {
    actor.GetNode<Damage>("/root/World/Damage").Spawn(actor, damage);
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
