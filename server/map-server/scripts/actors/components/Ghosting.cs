using Godot;
using System.Linq;
using System.Collections.Generic;

class Ghosting
{
  static PackedScene Scene = ResourceLoader.Load<PackedScene>("res://components/ghosting.tscn");

  Node3D actor;

  public Ghosting(Node3D actor)
  {
    this.actor = actor;

    var area = Scene.Instantiate<Area3D>();

    area.BodyEntered += BodyEntered;
    area.BodyExited += BodyExited;

    actor.AddChild(area);
  }

  void BodyEntered(Node3D body)
  {
    IActor actorTarget = (IActor)body;

    if (!nearestAnyActor.Contains(actorTarget.GetActorId()) && actorTarget != actor)
    {
      nearestAnyActor.Add(actorTarget.GetActorId());

      if (actorTarget.GetActorType() == ActorType.Player)
      {
        nearestPlayer.Add(actorTarget.GetActorId());
      }
    }
  }

  void BodyExited(Node3D body)
  {
    IActor actorTarget = (IActor)body;

    if (nearestAnyActor.Contains(actorTarget.GetActorId()) && actorTarget != actor)
    {
      nearestAnyActor.Remove(actorTarget.GetActorId());
    }
  }
}
