using System.Collections.Generic;
using Godot;

enum CompanyType
{
  Equipment,
  Food,
  Furniture,
  Stock
}

struct Supply
{
  public int ItemID;
  public int Amount;
  public int Required;
}

partial class Company : Node3D, IActorZone
{
  [Export]
  public int ID;

  [Export]
  public int Level;

  [Export]
  public CompanyType Type;

  public List<Quest> Quests;
  public int Money;
  public Supply[] supplies;
  public int Experience;

  public int GetActorID()
  {
    return int.Parse(Name);
  }

  public Vector3 GetActorPosition()
  {
    return Position;
  }

  public ActorType GetActorType()
  {
    return ActorType.Npc;
  }

  public Variant GetData()
  {
    return new Godot.Collections.Array()
    {
      ID,
      Money,
      Level,
      (int)Type
    };
  }

  public void TakeDamage(int actorId, int value)
  {
    throw new System.NotImplementedException();
  }

  public override void _Process(double delta)
  {

  }
}
