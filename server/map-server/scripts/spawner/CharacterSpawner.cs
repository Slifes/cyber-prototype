using Godot;
using System.Collections.Generic;

partial class CharacterSpawner : Node3D
{
  static CharacterSpawner instance;

  public static CharacterSpawner Instance { get { return instance; } }

  PackedScene playerScene;

  Dictionary<int, Player> players;

  public override void _Ready()
  {
    playerScene = ResourceLoader.Load<PackedScene>("res://actors/player_session.tscn");

    instance = this;
  }

  public IActor Spawn(Variant name, Variant position, Variant data)
  {
    if (!HasNode(name.ToString()))
    {
      var player = playerScene.Instantiate<Player>();

      player.Name = name.ToString();

      player.SetServerData(data);

      AddChild(player);

      player.GlobalPosition = (Vector3)position;

      //players.Add(player.GetActorId(), player);

      return player;
    }

    return null;
  }

  public void Unspawn(Variant name)
  {
    if (HasNode(name.ToString()))
    {
      GetNode(name.ToString()).QueueFree();
    }
  }
}
