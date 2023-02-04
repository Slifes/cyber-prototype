using Godot;
using System;

class NPCManager
{
  public static BaseNpcActor Instantiate(int ID)
  {
    var resource = ResourceLoader.Load<Npc>(String.Format("res://resources/npcs/{0}.tscn", ID));
    var node = ResourceLoader.Load<BaseNpcActor>("res://actors/BaseNPC.tscn");


    return new BaseNpcActor();
  }
}
