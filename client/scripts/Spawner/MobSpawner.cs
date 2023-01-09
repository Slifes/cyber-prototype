using Godot;

partial class MobSpawner: Node3D
{
	public void Spawn(Variant name, Vector3 position, Variant data)
	{
		GD.Print("Mob: ", name);
	}

	public void Unspawn(Variant name)
	{
		if (HasNode(name.ToString()))
		{
			RemoveChild(GetNode(name.ToString()));
		}
	}
}
