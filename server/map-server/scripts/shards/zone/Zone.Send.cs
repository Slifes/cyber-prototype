partial class Zone
{
  public static void SendActorEnteredZone(ZoneActor actor, ZoneActor target)
  {
    Instance.Rpc("ActorEnteredZone", actor.GetActorID(), target.GetActorID(), (int)target.GetActorType(), target.Position, target.Rotation.Y, target.GetData());
  }

  public static void SendActorExitedZone(ZoneActor actor, ZoneActor target)
  {
    Instance.Rpc("ActorExitedZone", actor.GetActorID(), target.GetActorID(), (int)target.GetActorType());
  }

  public static void SendActorEffect(int actorId, ActorType actorType, EffectType type, int value)
  {
    Instance.Rpc("ActorEffect", actorId, (int)actorType, (int)type, value);
  }

  public static void SendActorDrop(int actorId, ActorType actorType, int money, Godot.Collections.Array items)
  {
    Instance.Rpc("ActorDrop", actorId, (int)actorType, money, items);
  }
}
