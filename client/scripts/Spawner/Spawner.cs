using Godot;
using System.Collections.Generic;

partial class Spawner : Node
{
  static Spawner _instance;

  public static Spawner Instance { get { return _instance; } }

  Dictionary<ActorType, IActorSpawner> spawners;

  public override void _Ready()
  {
    _instance = this;

    spawners = new()
    {
      {ActorType.Player, GetNode<PlayerSpawner>("players")},
      {ActorType.Npc, GetNode<NpcSpawner>("npcs")}
    };
  }

  public IActor GetActor(string id, ActorType actorType)
  {
    return ((Node)spawners[actorType]).GetNode<IActor>(id);
  }

  public void Spawn(Packets.Server.SMActorEnteredZone command)
  {
    spawners[(ActorType)command.ActorType].Spawn(command);
  }

  public void Unspawn(Packets.Server.SMActorExitedZone command)
  {
    spawners[(ActorType)command.ActorType].Despawn(command);
  }
}
