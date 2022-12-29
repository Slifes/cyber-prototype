using Godot;
using System;

public partial class Spawner : MultiplayerSpawner
{
	PackedScene playerResource;

	public override void _Ready()
	{
		playerResource = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
	}

	public override Node _SpawnCustom(Variant data)
	{
		var player = playerResource.Instantiate();

		// GD.Print(data);

		player.Name = data.ToString();

		return player;
	}
}
