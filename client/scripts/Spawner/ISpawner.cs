interface IActorSpawner
{
  IActor Spawn(Packets.Server.SMActorEnteredZone command);
  void Despawn(Packets.Server.SMActorExitedZone command);
}
