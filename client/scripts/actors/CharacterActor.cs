using System;
using Godot;

partial class CharacterActor : CharacterBody3D, IActor
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

  public virtual void ConsumeSP(int sp)
  {
    currentSP -= sp;
  }

  public virtual void SetServerData(Variant data)
  {
    var dataArray = data.AsGodotArray<Variant>();

    currentHP = (int)dataArray[0];
    currentSP = (int)dataArray[1];
    maxHP = (int)dataArray[2];
    maxSP = (int)dataArray[3];
  }

  public ActorType GetActorType()
  {
    return ActorType.Player;
  }

  public virtual void ExecuteSkill(Variant skillId) { }
}
