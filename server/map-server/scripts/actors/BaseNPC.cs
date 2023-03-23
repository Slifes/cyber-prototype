using Godot;

partial class BaseNPC : ZoneActor
{
  [Export]
  public int ID;

  protected int currentHP = 100;

  protected int maxHP = 100;

  public override ActorType GetActorType()
  {
    return ActorType.Npc;
  }

  public int GetCurrentHP()
  {
    return currentHP;
  }

  public int GetMaxHP()
  {
    return maxHP;
  }

  public override Variant GetData()
  {
    var data = new Godot.Collections.Array<Variant>()
    {
      ID,
      currentHP,
      maxHP,
    };

    return data;
  }

  public override void TakeDamage(int actorId, int damage)
  {
    currentHP -= damage;

    base.TakeDamage(actorId, damage);
  }
}
