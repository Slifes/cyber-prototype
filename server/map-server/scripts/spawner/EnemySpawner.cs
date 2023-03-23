using Godot;

partial class EnemySpawner : Node3D
{
  [Export]
  public PackedScene Mob;

  [Export]
  public int MobCount;

  public override void _Ready()
  {
    if (!Multiplayer.IsServer()) return;

    var timer = new Timer();

    timer.WaitTime = 2;
    timer.Timeout += SpawnMobs;
    timer.Autostart = true;

    AddChild(timer);
  }

  void SpawnMobs()
  {
    GD.Print("Spawn COunt: ", GetChildCount());

    if (GetChildCount() < MobCount)
    {
      var mob = Mob.Instantiate<Node3D>();

      mob.Name = Multiplayer.MultiplayerPeer.GenerateUniqueId().ToString();
      mob.Position = new Vector3(GD.Randf() * -5, 1, GD.Randf() * 5);

      AddChild(mob);
    }
  }
}
