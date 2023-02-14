using Godot;
using System.Collections.Generic;

partial class Zone : BaseShard
{
  static PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://actors/player_zone.tscn");

  public Dictionary<int, List<int>> neraests;

  private static Zone _instance;

  public static Zone Instance { get { return _instance; } }

  public override void _Ready()
  {
    base._Ready();

    neraests = new();

    if (Multiplayer.IsServer())
    {
      _instance = this;

      SkillManager.CreateInstance("skills");
      SkillManager.Instance.Load();
    }
  }

  public List<int> GetPlayerNearest(int actorId)
  {
    if (neraests.ContainsKey(actorId))
    {
      return neraests[actorId];
    }

    return new List<int>();
  }
}
