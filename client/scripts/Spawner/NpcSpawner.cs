﻿using Godot;

partial class NpcSpawner : Node3D
{
  public void Spawn(Variant name, Vector3 position, Variant data)
  {
    var d = data.AsGodotArray();

    GD.Print("Spawn: ", name);
    GD.Print("Data: ", data);

    if (d.Count > 0)
    {
      var reference = (int)d[0];

      if (reference == 0)
      {
        var p = ResourceLoader.Load<PackedScene>("res://actors/mobs/kirt.tscn");

        var n = p.Instantiate<CharacterActor>();

        n.Name = name.ToString();

        AddChild(n);

        n.GlobalPosition = position;
        n.SetServerData(data);
      }
    }
  }

  public void Unspawn(Variant name)
  {
    if (HasNode(name.ToString()))
    {
      GetNode(name.ToString()).QueueFree();
    }
  }
}
