using Godot;

partial class BaseNPC : CharacterBody3D, IActorZone
{
  [Export]
  public int ID;

  public int GetActorID()
  {
    throw new System.NotImplementedException();
  }

  public ActorType GetActorType()
  {
    return ActorType.Npc;
  }

  public Vector3 GetActorPosition() { return Vector3.Zero; }

  public int GetCurrentHP()
  {
    throw new System.NotImplementedException();
  }

  public int GetCurrentSP()
  {
    throw new System.NotImplementedException();
  }

  public virtual Variant GetData()
  {
    var data = new Godot.Collections.Array<Variant>()
    {
      ID,
      // currentHP,
      // currentSP,
      // maxHP,
      // maxSP,
    };

    return data;
  }

  public int GetMaxHP()
  {
    throw new System.NotImplementedException();
  }

  public int GetMaxSP()
  {
    throw new System.NotImplementedException();
  }

  public void onActorReady()
  {
    throw new System.NotImplementedException();
  }

  public void SetServerData(Variant data)
  {
    throw new System.NotImplementedException();
  }

  public void TakeDamage(int damage)
  {
    throw new System.NotImplementedException();
  }
}
