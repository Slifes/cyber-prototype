using System.Collections.Generic;

class Nearests
{
  public static List<int> EMPTY_LIST = new List<int>();

  Dictionary<int, List<int>> nearests;

  List<int> peers = new();

  public Nearests()
  {
    nearests = new();
  }

  public List<int> GetPeers()
  {
    return peers;
  }

  public List<int> GetPlayerNearest(int actorId)
  {
    if (nearests.ContainsKey(actorId))
    {
      return nearests[actorId];
    }

    return EMPTY_LIST;
  }

  public void CreateActorList(int actorId)
  {
    if (!nearests.ContainsKey(actorId))
    {
      peers.Add(actorId);
      nearests.Add(actorId, new());
    }
  }

  public void RemoveActorList(int actorId)
  {
    if (nearests.ContainsKey(actorId))
    {
      peers.Remove(actorId);
      nearests.Remove(actorId);
    }
  }

  public void Add(int actorId, int actorTargetId, ActorType type)
  {
    switch (type)
    {
      case ActorType.Player:
        nearests[actorId].Add(actorTargetId);
        break;
      case ActorType.Npc:
        if (!nearests.ContainsKey(actorTargetId))
          nearests.Add(actorTargetId, new()
          {
            actorId
          });
        else
          nearests[actorTargetId].Add(actorId);
        break;
    }
  }

  public void Remove(int actorId, int actorTargetId, ActorType type)
  {
    switch (type)
    {
      case ActorType.Player:
        nearests[actorId].Remove(actorTargetId);
        break;
      case ActorType.Npc:
        if (nearests.ContainsKey(actorTargetId))
          nearests[actorTargetId].Remove(actorId);
        break;
    }
  }
}
