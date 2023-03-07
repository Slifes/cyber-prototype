interface IActorZone
{
  int GetActorID();

  ActorType GetActorType();

  Godot.Vector3 GetActorPosition();

  // float GetActorYaw();

  Godot.Variant GetData();

  void TakeDamage(int value);
}
