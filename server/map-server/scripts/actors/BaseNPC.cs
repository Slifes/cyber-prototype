using Godot;

partial class BaseNPC : CharacterBody3D, IActorZone
{
  [Export]
  public int ID;

  int _actorId;

  int currentHP = 100;

  public int GetActorID()
  {
    return _actorId;
  }

  public ActorType GetActorType()
  {
    return ActorType.Npc;
  }

  public Vector3 GetActorPosition() { return Vector3.Zero; }

  public int GetCurrentHP()
  {
    return 100;
  }

  public int GetCurrentSP()
  {
    return 100;
  }

  public virtual Variant GetData()
  {
    var data = new Godot.Collections.Array<Variant>()
    {
      ID,
      currentHP,
      100,
      100,
      100
    };

    return data;
  }

  public int GetMaxHP()
  {
    return 100;
  }

  public int GetMaxSP()
  {
    return 100;
  }

  public void onActorReady()
  {
    _actorId = int.Parse(Name);
  }

  public void SetServerData(Variant data)
  {

  }

  public void TakeDamage(int damage)
  {
    currentHP -= damage;
  }
}
