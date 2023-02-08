using System;
using Godot;

partial class CharacterSpawner : Node3D
{
  PackedScene playerScene;

  public override void _Ready()
  {
    playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
  }

  public void Spawn(Variant name, Vector3 position, float yaw, Variant data)
  {
    if (!HasNode(name.ToString()))
    {
      var player = playerScene.InstantiateOrNull<Player>();

      if (player != null)
      {
        player.Name = name.ToString();
        player.SetMultiplayerAuthority(Int32.Parse(name.ToString()));
        player.SetServerData(data);

        AddChild(player);

        player.GlobalPosition = position;
        player.Rotation = new Vector3(0, yaw, 0);
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
