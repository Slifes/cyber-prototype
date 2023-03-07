using Godot;
using System.Collections.Generic;

partial class Spawner : Node
{
  static Spawner _instance;

  public static Spawner Instance { get { return _instance; } }

  Dictionary<ActorType, IActorSpawner> spawners;

  Dictionary<int, IActor> actors;

  public override void _Ready()
  {
    _instance = this;

    actors = new();

    spawners = new()
    {
      {ActorType.Player, GetNode<PlayerSpawner>("players")},
      {ActorType.Npc, GetNode<NpcSpawner>("npcs")}
    };
  }

  public T GetActor<T>(int id)
  {
    IActor actor;

    actors.TryGetValue(id, out actor);

    return (T)actor;
  }

  public void Spawn(Packets.Server.SMActorEnteredZone command)
  {
    var actor = spawners[(ActorType)command.ActorType].Spawn(command);

    if (actor != null)
    {
      actors.Add(command.ActorId, actor);
    }
  }

  public void Unspawn(Packets.Server.SMActorExitedZone command)
  {
    spawners[(ActorType)command.ActorType].Despawn(command);

    actors.Remove(command.ActorId);
  }
}
