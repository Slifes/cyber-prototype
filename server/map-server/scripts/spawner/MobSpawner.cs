using Godot;

partial class MobSpawner : Node3D
{
  [Export]
  NodePath targetSpawnPath;

  [Export]
  PackedScene MobScene;

  [Export]
  int MobCount;

  [Export]
  float X;

  [Export]
  float Y;

  Node3D targetSpawn;

  double LastTimeChecked;

  int currentCount = 0;

  public override void _Ready()
  {
	targetSpawn = GetNode<Node3D>(targetSpawnPath);
  }

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
	if (currentCount < MobCount)
	{
	  Spawn();
	}
  }

  private void Spawn()
  {
	var random = new RandomNumberGenerator();

	var mobX = random.Randf() * X;
	var mobZ = random.Randf() * Y;

	var instance = MobScene.Instantiate<Node3D>();

	instance.Name = Multiplayer.MultiplayerPeer.GenerateUniqueId().ToString();

	targetSpawn.AddChild(instance);

	instance.GlobalPosition = new Vector3(mobX, instance.GlobalPosition.Y, mobZ);

	currentCount++;

  }
}
