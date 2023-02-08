using Godot;
using System;

class NPCManager
{
  public static BaseNPC Create(int ID)
  {
    //var resource = ResourceLoader.Load<Npc>(String.Format("res://resources/npcs/{0}.tscn", ID));
    var node = ResourceLoader.Load<PackedScene>("res://npcs/BaseNPC.tscn");

    var npc = (BaseNPC)node.Instantiate();

    //npc.StartComponents(resource.Component);

    // AnimationLibrary library = npc.Animation.GetAnimationLibrary("");

    // foreach (var animation in resource.Animations)
    // {
    //   library.AddAnimation(animation.ResourceName, animation);
    // }

    return npc;
  }
}
