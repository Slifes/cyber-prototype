using Godot;
using System;

partial class NpcSpawner: ActorSpawner
{
  public override void Spawn(Packets.Server.SMActorEnteredZone command)
  {
    // var d = data.AsGodotArray();

    // GD.Print("Spawn: ", name);
    // GD.Print("Data: ", data);

    // if (d.Count == 0) return;

    // var npcId = (int)d[0];

    // GD.Print("NpcId: ", npcId);

    // var scene = ResourceLoader.Load<PackedScene>(String.Format("res://resources/npcs/{0}.tscn", npcId));

    // var actor = scene.Instantiate<BaseNPC>();

    // actor.Name = name.ToString();

    // CallDeferred("add_child", actor);
    // CallDeferred("UpdateActor", actor, (Vector3)position, (float)yaw, data);
  }

  void UpdateActor(CharacterActor actor, Vector3 position, float yaw, Variant data)
  {
    actor.Position = position;
    actor.Rotation = new Vector3(0, yaw, 0);
    actor.SetServerData(data);
  }
}
