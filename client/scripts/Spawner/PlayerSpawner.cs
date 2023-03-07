using Godot;

partial class PlayerSpawner : ActorSpawner
{
  PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");

  public override IActor Spawn(Packets.Server.SMActorEnteredZone command)
  {
    if (!HasNode(command.ActorId.ToString()))
    {
      var player = playerScene.Instantiate<Player>();

      player.Name = command.ActorId.ToString();
      player.SetMultiplayerAuthority(command.ActorId);
      player.Position = new Vector3(command.Position[0], command.Position[1], command.Position[2]);
      player.Rotation = new Vector3(0, command.Yaw, 0);

      CallDeferred("add_child", player);

      return player;
    }

    return null;
  }
}
