using Godot;

enum ActorType
{
  Player,
  Npc
}

interface IActor
{
  ActorType GetActorType();

  int GetCurrentHP();

  int GetCurrentSP();

  int GetMaxHP();

  int GetMaxSP();
  
  void TakeDamage(int damage);

  //void ConsumeSP(int sp);

  void onActorReady();

  int GetActorId();

  void SetServerData(Variant data);

  void ExecuteSkill(Variant skillId);
}
