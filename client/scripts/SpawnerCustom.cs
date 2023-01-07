using System;
using Godot;

partial class SpawnerCustom: Node3D
{
	PackedScene playerScene;

	public override void _Ready()
	{
		playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
	}

	public void SpawnPlayableActor(Variant name, Vector3 position)
	{
		if (!HasNode(name.ToString()))
		{
			var player = playerScene.InstantiateOrNull<Player>();

			if (player != null)
			{
				player.Name = name.ToString();
				player.InitialPosition = position;
				player.SetMultiplayerAuthority(Int32.Parse(name.ToString()));

				AddChild(player);
			} else
			{
				GD.Print("Failed to instantiate a player!");
			}
		}
	}

	public void Spawn(Variant name, Vector3 position)
	{
		if (!HasNode(name.ToString()))
		{
			var player = playerScene.InstantiateOrNull<Player>();

			if (player != null)
			{
				player.Name = name.ToString();
				player.InitialPosition = position;

				AddChild(player);
			}

			GD.Print("Instante");
		}
	}

	public void Unspawn(Variant name)
	{
		if (HasNode(name.ToString()))
		{
			RemoveChild(GetNode(name.ToString()));
		}
	}
}
