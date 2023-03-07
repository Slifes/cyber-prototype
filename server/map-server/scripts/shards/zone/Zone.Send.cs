partial class Zone
{
  public static void SendActorEnteredZone(ZoneActor actor, ZoneActor target)
  {
    Instance.Rpc("ActorEnteredZone", actor.GetActorID(), target.GetActorID(), (int)target.GetActorType(), target.Position, target.Rotation.Y, target.GetData(), actor.GetActorType() == ActorType.Player);
  }

  public static void SendActorExitedZone(ZoneActor actor, ZoneActor target)
  {
    Instance.Rpc("ActorExitedZone", actor.GetActorID(), target.GetActorID(), (int)target.GetActorType(), actor.GetActorType() == ActorType.Player);
  }

  public static void SendActorDamage(int actorId, int actorType, int value)
  {
    Instance.Rpc("ActorTakeDamage", actorId, actorType, value);
  }
}
