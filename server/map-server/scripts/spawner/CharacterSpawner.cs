using Godot;

partial class CharacterSpawner : Node3D
{
  PackedScene playerScene;

  public override void _Ready()
  {
    playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
  }

  public IActor Spawn(Variant name, Variant position, Variant data)
  {
    if (!HasNode(name.ToString()))
    {
      var player = playerScene.InstantiateOrNull<Player>();

      if (player == null)
      {
        GD.Print("Failed to instantiate player");
        return null;
      }

      player.Name = name.ToString();

      player.SetServerData(data);

      AddChild(player);

      player.GlobalPosition = (Vector3)position;

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
