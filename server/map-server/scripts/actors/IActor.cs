using Godot;

enum ActorType
{
  Player,
  Npc
}

interface IActor
{
  void onActorReady();

  int GetActorId();

  ActorType GetActorType();

  int GetCurrentHP();

  int GetCurrentSP();

  int GetMaxHP();

  int GetMaxSP();

  void TakeDamage(int damage);

  void SetServerData(Variant data);

  Variant GetData();
}
