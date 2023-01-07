using Godot;

partial class SpawnerCustom : Node3D
{
	PackedScene playerScene;

	public override void _Ready()
	{
		playerScene = ResourceLoader.Load<PackedScene>("res://actors/Player.tscn");
	}

	public void Spawn(Variant name)
	{
		if (!HasNode(name.ToString()))
		{
			var player = playerScene.InstantiateOrNull<Player>();

			if (player == null)
			{
				GD.Print("Failed to instantiate player");
				return;
			}

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
