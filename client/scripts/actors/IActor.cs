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

  void onActorReady();

  int GetActorId();

  void SetServerData(Godot.Collections.Array dataArray);
}
