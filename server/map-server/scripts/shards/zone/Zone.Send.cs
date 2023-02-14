using Godot;

partial class Zone
{
  public static void SendActorEnteredZone(IActorZone actor, IActorZone target)
  {
    Instance.Rpc("ActorEnteredZone", actor.GetActorID(), target.GetActorID(), (int)target.GetActorType());
  }
  
  public static void SendActorExitedZone(ZoneActor actor, ZoneActor target)
  {
    Instance.Rpc("ActorExitedZone", actor.GetActorID(), target.GetActorID(), (int)target.GetActorType());
  }
}