using Godot;
using System.Collections.Generic;

partial class ShardSpawner : Node
{
  [Export]
  PackedScene actorScene;

  Dictionary<int, Node3D> actors;

  public override void _Ready()
  {
    actors = new();
  }

  public void Spawn(int actorId, Vector3 position, float yaw)
  {
    var instance = actorScene.Instantiate<Node3D>();

    instance.Name = actorId.ToString();

    AddChild(instance);

    instance.Position = position;
    instance.Rotation = new Vector3(0, yaw, 0);

    actors.Add(actorId, instance);
  }

  public void Despawn(int actorId)
  {
    if (actors.ContainsKey(actorId))
    {
      actors[actorId].QueueFree();
      actors.Remove(actorId);
    }
  }

  public Node3D Get(int actorId)
  {
    if (!actors.ContainsKey(actorId))
    {
      return null;
    }

    return actors[actorId];
  }
}
