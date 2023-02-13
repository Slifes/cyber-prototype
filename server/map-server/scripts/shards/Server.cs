using Godot;
using System.Collections.Generic;

partial class ShardFactory : Node
{
  void CreateShard(string zone)
  {
    // if (zones.ContainsKey(zone))
    //   return;

    var pid = OS.CreateProcess(OS.GetExecutablePath(), new string[] {"--headless", "shard", zone}, true);
 
    var zonePackedScene = ResourceLoader.Load<PackedScene>(string.Format("res://zones/{0}.tscn", zone));
    var zoneInstance = zonePackedScene.Instantiate<ShardConnect>();

    zoneInstance.IsServer = false;
    zoneInstance.PID = pid;

    AddChild(zoneInstance); 

    // zones.Add(zone, zoneInstance);
  }

  void DropShard(string zone)
  {
    if (zones.ContainsKey(zone))
    {
      zones[zone].QueueFree();

      OS.Kill(zones[zone].PID);

      zones.Remove(zone);
    }
  }
}
