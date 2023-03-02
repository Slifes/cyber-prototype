using Godot;
using System.Collections.Generic;

class ActorManager<T> where T : Node
{
  public static ActorManager<T> Instance { get { return _instance; } }

  private static ActorManager<T> _instance;

  Dictionary<int, T> actors;

  public ActorManager()
  {
    actors = new();

    _instance = this;
  }

  public void AddActor(int actorId, T actor)
  {
    if (!actors.ContainsKey(actorId))
    {
      actors.Add(actorId, actor);
    }
  }

  public T GetActor(int actorId)
  {
    T actor = null;

    actors.TryGetValue(actorId, out actor);

    return actor;
  }

  public void RemoveActor(int actorId)
  {
    if (actors.ContainsKey(actorId))
    {
      actors.Remove(actorId);
    }
  }
}
