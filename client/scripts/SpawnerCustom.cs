using Godot;

partial class SpawnerCustom: Node3D
{
	PackedScene playerScene;

	public override void _Ready()
	{
		playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
	}

	public void Spawn(Variant name, Vector3 position)
	{
		if (!HasNode(name.ToString()))
		{
			var player = (CharacterBody3D)playerScene.Instantiate();

			player.Name = name.ToString();
			// player.GlobalPosition = position;

			AddChild(player);
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
