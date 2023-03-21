using Godot;

partial class Heal : Node
{
  public IActorZone Actor;

  [Export]
  public int Value;

  public override void _Ready()
  {
    Zone.SendActorEffect(Actor.GetActorID(), Actor.GetActorType(), EffectType.Heal, 10);

    QueueFree();
  }
}
