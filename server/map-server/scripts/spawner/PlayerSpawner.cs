using Godot;

partial class PlayerSpawner : Node
{
  static PlayerSpawner instance;

  public static PlayerSpawner Instance { get { return instance; } }

  PackedScene playerScene;

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
      player.Position = (Vector3)position;

      AddChild(player);

      SessionManager.Instance.AddActor(int.Parse(player.Name), player);

      return player;
    }

    return null;
  }

  public void Despawn(long id)
  {
    if (HasNode(id.ToString()))
    {
      GetNode(id.ToString()).QueueFree();

      SessionManager.Instance.RemoveActor((int)id);
    }
  }
}
