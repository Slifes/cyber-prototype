using Godot;

partial class MobSpawner: Node3D
{
	public void Spawn(Variant name, Vector3 position, Variant data)
	{
		var d = data.AsGodotArray();

		GD.Print("Spawn: ", name);
		GD.Print("Data: ", data);

		if (d.Count > 0)
		{
			var reference = (int)d[0];

			if (reference == 0)
			{
				var p = ResourceLoader.Load<PackedScene>("res://actors/mobs/kirt.tscn");

				var n = p.Instantiate<MobActor>();

				n.Name = name.ToString();

				AddChild(n);

				n.GlobalPosition = position;
				n.SetData(data);
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
