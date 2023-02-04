using Godot;

partial class NpcSpawner : Node3D
{
  public void Spawn(Variant name, Vector3 position, Variant data)
  {
    var d = data.AsGodotArray();

    GD.Print("Spawn: ", name);
    GD.Print("Data: ", data);

    if (d.Count == 0) return;

    var npcId = (int)d[0];

    // var actor = NPCManager.Instantiate(npcId);

    // actor.Name = name.ToString();

    // AddChild(actor);

    // actor.GlobalPosition = position;
    // actor.SetServerData(data);
  }

  public void Unspawn(Variant name)
  {
    if (HasNode(name.ToString()))
    {
      GetNode(name.ToString()).QueueFree();
    }
  }
}
