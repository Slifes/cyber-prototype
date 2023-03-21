using Godot;
using System;

class NPCManager
{
  public static BaseNPC Create(int ID)
  {
    var node = ResourceLoader.Load<PackedScene>("res://npcs/BaseNPC.tscn");

    var npc = (BaseNPC)node.Instantiate();

    return npc;
  }
}
