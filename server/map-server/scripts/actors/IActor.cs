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

    void onActorReady();

    int GetActorId();

    void SetServerData(Variant data);

    Variant GetData();
}
