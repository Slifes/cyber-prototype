using Godot;
using System.Collections.Generic;

partial class Zone : BaseShard
{
  static PackedScene packedScene = ResourceLoader.Load<PackedScene>("res://actors/player_zone.tscn");

  public Dictionary<int, List<int>> neraests;

  public Dictionary<int, List<int>> nearestsPlayer;

  private static Zone _instance;

  public static Zone Instance { get { return _instance; } }

  public static List<int> EMPTY_LIST = new List<int>();

  public override void _Ready()
  {
    base._Ready();

    neraests = new();

    nearestsPlayer = new();

    if (GetParent<ShardConnect>().IsServer)
    {
      _instance = this;
    }
  }

  public List<int> GetPlayerNearest(int actorId)
  {
    if (nearestsPlayer.ContainsKey(actorId))
    {
      return nearestsPlayer[actorId];
    }

    return EMPTY_LIST;
  }

  void AddToNearestPlayer(int peerId, int actorId)
  {
    if (!nearestsPlayer.ContainsKey(peerId))
    {
      nearestsPlayer.Add(peerId, new List<int>() { actorId });
    }
    else
    {
      nearestsPlayer[peerId].Add(actorId);
    }
  }

  void AddToNearestActors(int peerId, int actorId)
  {
    if (!neraests.ContainsKey(peerId))
    {
      neraests.Add(peerId, new List<int>() { actorId });
    }
    else
    {
      neraests[peerId].Add(actorId);
    }
  }

  void RemoveFromNearestPlayer(int peerId, int actorId)
  {
    if (nearestsPlayer.ContainsKey(peerId))
    {
      nearestsPlayer[peerId].Remove(actorId);
    }
  }

  void RemoveFromNearestActors(int peerId, int actorId)
  {
    if (neraests.ContainsKey(peerId))
    {
      neraests[peerId].Remove(actorId);
    }
  }

  void AddActorToNearest(int peerId, int actorId, ActorType type)
  {
    if (type == ActorType.Player)
    {
      AddToNearestPlayer(peerId, actorId);
    }
    else
    {
      AddToNearestActors(peerId, actorId);
      AddToNearestPlayer(actorId, peerId);
    }
  }

  void RemoveActorFromNearests(int peerId, int actorId, ActorType type)
  {
    if (type == ActorType.Player)
    {
      RemoveFromNearestPlayer(peerId, actorId);
    }
    else
    {
      RemoveFromNearestActors(peerId, actorId);
      RemoveFromNearestPlayer(actorId, peerId);
    }
  }
}
