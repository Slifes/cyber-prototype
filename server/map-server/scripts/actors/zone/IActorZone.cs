interface IActorZone
{
  int GetActorID();

  ActorType GetActorType();

  Godot.Vector3 GetActorPosition();

  Godot.Variant GetData();

  void TakeDamage(int actorId, int value);
}
