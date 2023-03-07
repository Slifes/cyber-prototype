using Godot;
using System;

partial class NpcSpawner : ActorSpawner
{
  public override IActor Spawn(Packets.Server.SMActorEnteredZone command)
  {
    var data = Variant.CreateFrom(command.Data);
    var d = data.AsGodotArray();

    var npcId = (int)d[0];

    GD.Print("NpcId: ", npcId);
    GD.Print("Spawn: ", command.ActorId);
    GD.Print("Data: ", command.Data);

    var scene = ResourceLoader.Load<PackedScene>(String.Format("res://resources/npcs/{0}.tscn", npcId));

    var actor = scene.Instantiate<BaseNPC>();

    actor.Name = command.ActorId.ToString();
    actor.Position = new Vector3(command.Position[0], command.Position[1], command.Position[2]);
    actor.Rotation = new Vector3(0, command.Yaw, 0);
    actor.SetServerData(data);

    CallDeferred("add_child", actor);

    return null;
  }
}
