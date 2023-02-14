using Godot;
using System.Linq;
using System.Collections.Generic;

class Ghosting
{
  static PackedScene Scene = ResourceLoader.Load<PackedScene>("res://components/ghosting.tscn");

  ZoneActor actor;

  public Ghosting(ZoneActor actor)
  {
    this.actor = actor;

    var area = Scene.Instantiate<Area3D>();

    area.BodyEntered += BodyEntered;
    area.BodyExited += BodyExited;

    actor.AddChild(area);
  }

  void BodyEntered(Node3D body)
  {
    ZoneActor actorTarget = (ZoneActor)body;

    Zone.SendActorEnteredZone(actor, actorTarget);
  }

  void BodyExited(Node3D body)
  {
    ZoneActor actorTarget = (ZoneActor)body;

    Zone.SendActorExitedZone(actor, actorTarget);
  }
}
