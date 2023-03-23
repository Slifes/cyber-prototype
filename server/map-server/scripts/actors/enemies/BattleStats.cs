using System.Collections.Generic;

struct BattleDamage
{
  public int actorId;
  public int Damage;
}

class BattleStats
{
  public ZoneActor Self;

  public ZoneActor TargetActor;

  Dictionary<int, List<BattleDamage>> Damage = new();

  public BattleStats(ZoneActor actor)
  {
    this.Self = actor;
  }

  public void SetTargetActor(ZoneActor actor)
  {
    this.TargetActor = actor;
  }

  // Add damage to the list
  public void ComputeDamage(int actorId, int damage)
  {
    if (Damage.ContainsKey(actorId))
    {
      Damage[actorId].Add(new BattleDamage
      {
        actorId = actorId,
        Damage = damage
      });
    }
    else
    {
      Damage.Add(actorId, new List<BattleDamage>
      {
        new BattleDamage
        {
          actorId = actorId,
          Damage = damage
        }
      });
    }
  }

  // Get the actor with the highest damage
  public int GetActorByMaxDamage()
  {
    int maxDamage = 0;
    int actorId = 0;

    foreach (var item in Damage)
    {
      int damage = 0;

      foreach (var damageItem in item.Value)
      {
        damage += damageItem.Damage;
      }

      if (damage > maxDamage)
      {
        maxDamage = damage;
        actorId = item.Key;
      }
    }

    return actorId;
  }

  public void ClearDamage()
  {
    Damage.Clear();
  }
}
