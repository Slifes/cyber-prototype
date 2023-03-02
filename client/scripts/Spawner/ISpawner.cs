public interface IActorSpawner
{
  public void Spawn(Packets.Server.SMActorEnteredZone command);
  public void Despawn(Packets.Server.SMActorExitedZone command);
}
