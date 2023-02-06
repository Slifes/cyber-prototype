using Godot;
using System.Linq;
using System.Collections.Generic;

class Ghosting : IComponent
{
  static PackedScene Scene = ResourceLoader.Load<PackedScene>("res://components/ghosting.tscn");

  IActor actor;

  List<int> nearestAnyActor;

  List<int> nearestPlayer;

  public List<int> Nearest { get { return nearestAnyActor; } }

  public List<int> NearestPlayers { get { return nearestPlayer; } }

  public Ghosting(Node actor)
  {
    this.actor = (IActor)actor;

    var area = Scene.Instantiate<Area3D>();

    area.BodyEntered += BodyEntered;
    area.BodyExited += BodyExited;

    actor.AddChild(area);

    nearestAnyActor = new();
    nearestPlayer = new();

  }

  void BodyEntered(Node3D body)
  {
    IActor actorTarget = (IActor)body;

    if (!nearestAnyActor.Contains(actorTarget.GetActorId()))
    {
      nearestAnyActor.Add(actorTarget.GetActorId());

      ServerBridge.Instance.SendActorEnteredZone(actor.GetActorId(), actorTarget);

      if (actorTarget.GetActorType() == ActorType.Player)
      {
        nearestPlayer.Add(actorTarget.GetActorId());
      }
    }
  }

  void BodyExited(Node3D body)
  {
    IActor actorTarget = (IActor)body;

    if (nearestAnyActor.Contains(actorTarget.GetActorId()))
    {
      nearestAnyActor.Remove(actorTarget.GetActorId());

      ServerBridge.Instance.SendActorExitedZone(actor.GetActorId(), actorTarget);

      if (actorTarget.GetActorType() == ActorType.Player)
      {
        nearestPlayer.Remove(actorTarget.GetActorId());
      }
    }
  }
}
