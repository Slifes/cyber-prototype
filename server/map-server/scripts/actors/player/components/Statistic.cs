using System.Collections.Generic;

enum Stats
{
  Melee,
  Range,
  Defense,
  Fire,
  Ice,
  Critical
}

class Statistic
{
  Dictionary<Stats, int> stats;

  public Dictionary<Stats, int> Values { get { return stats; } }

  public Statistic(PlayerZone player)
  {
    stats = new();

    player.StatisticState += UpdateStaticData;
  }

  void UpdateStaticData(Godot.Collections.Dictionary<int, int> data)
  {

  }
}
