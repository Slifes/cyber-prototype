using Godot;

partial class Npc : Resource
{
  [Export]
  public string Name;

  [Export]
  public PackedScene Mesh;

  [Export]
  public Animation[] Animations;

  [Export]
  public Script[] Components;
}
