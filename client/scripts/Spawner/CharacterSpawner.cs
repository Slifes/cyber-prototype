using System;
using Godot;

partial class CharacterSpawner: Node3D
{
	PackedScene playerScene;

	public override void _Ready()
	{
		playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
	}

	public void Spawn(Variant name, Vector3 position, Variant data)
	{
		if (!HasNode(name.ToString()))
		{
			var player = playerScene.InstantiateOrNull<Player>();

			if (player != null)
			{
				player.Name = name.ToString();
				player.InitialPosition = position;
				player.SetMultiplayerAuthority(Int32.Parse(name.ToString()));
				player.SetServerData(data);

				AddChild(player);
			}
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
