using Godot;
using System;
using Godot.Collections;

partial class BodyActor : RigidBody3D, IActor
{
  protected int _actorId;

  protected ActorType _type;

  protected int currentHP;

  protected int currentSP;

  protected int maxHP;

  protected int maxSP;

  public void onActorReady()
  {
    _actorId = Int32.Parse(Name);

    maxHP = 100;
    maxSP = 100;

    currentHP = maxHP;
    currentSP = maxSP;
  }

  public int GetActorId()
  {
    return _actorId;
  }

  public int GetCurrentHP()
  {
    return currentHP;
  }

  public int GetCurrentSP()
  {
    return currentSP;
  }

  public int GetMaxHP()
  {
    return maxHP;
  }

  public int GetMaxSP()
  {
    return maxSP;
  }

  public void TakeDamage(int damage)
  {
    currentHP -= damage;
  }

  public void SetServerData(Variant data)
  {
    GD.Print(data);
  }

  public ActorType GetActorType()
  {
    return ActorType.Npc;
  }

  public virtual Variant GetData()
  {
    var data = new Array<Variant>()
    {
      0,
      currentHP,
      currentSP,
      maxHP,
      maxSP
    };

    return data;
  }
}
