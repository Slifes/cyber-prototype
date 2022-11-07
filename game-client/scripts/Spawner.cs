using Godot;
using System;

public partial class Spawner : MultiplayerSpawner
{
	PackedScene playerResource;

	public override void _Ready()
	{
		playerResource = ResourceLoader.Load<PackedScene>("res://Player.tscn");
	}

	public override Node _SpawnCustom(Variant data)
	{
		var player = playerResource.Instantiate();

		player.Name = data.ToString();

		return player;
	}
}
