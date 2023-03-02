using System;
using Godot;
using Godot.Collections;

partial class SessionActor : Node3D, IActor
{
  protected int _actorId;

  protected ActorType _type;

  protected ActorState state;

  protected int currentHP;

  protected int currentSP;

  protected int maxHP;

  protected int maxSP;

  public float Yaw;

  public void onActorReady()
  {
    _actorId = Int32.Parse(Name);

    maxHP = 100;
    maxSP = 100;

    currentHP = maxHP;
    currentSP = maxSP;
  }

  public override void _Ready()
  {
    onActorReady();
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

  public virtual void TakeDamage(int damage)
  {
    currentHP -= damage;
  }

  public void SetServerData(Variant data)
  {
    var dataArray = data.AsGodotArray<Variant>();

    currentHP = (int)dataArray[1];
    currentSP = (int)dataArray[2];
    maxHP = (int)dataArray[3];
    maxSP = (int)dataArray[4];
  }

  public virtual Variant GetData()
  {
    var data = new Array<Variant>()
    {
      currentHP,
      currentSP,
      maxHP,
      maxSP
    };

    return data;
  }

  public virtual ActorType GetActorType()
  {
    return ActorType.Player;
  }
}
