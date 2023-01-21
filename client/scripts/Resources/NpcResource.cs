using Godot;
using Godot.Collections;

partial class NpcResource: Resource
{
  [Export]
  public int ID;

  [Export]
  public Array<Variant> behaviors;
}