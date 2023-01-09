using Godot;

partial class MobSpawner : Node3D
{
	[Export]
	PackedScene MobScene;

	[Export]
	int MobCount;

	[Export]
	float X;

	[Export]
	float Y;

	double LastTimeChecked;

	public override void _PhysicsProcess(double delta)
	{

		var now = Time.GetUnixTimeFromSystem();

		if ((now - LastTimeChecked) > 2)
		{
			HandlerSpawn();

			LastTimeChecked = now;
		}
	}

	private void HandlerSpawn()
	{
		var count = GetChildCount();

		if (count < MobCount)
		{
			Spawn();
		}
	}

	private void Spawn()
	{
		var random = new RandomNumberGenerator();

		var mobX = random.Randf() * X;
		var mobY = random.Randf() * Y;

		var instance = MobScene.Instantiate<Node3D>();

		instance.Name = Multiplayer.MultiplayerPeer.GenerateUniqueId().ToString();

		AddChild(instance);

		instance.GlobalPosition = new Vector3(mobX, 0, mobY);
	}
}
