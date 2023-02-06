using Godot;


enum ComponentType
{
  Agressive,
  Dialogue
}

partial class Npc : Resource
{
  [Export]
  public string Name;

  [Export]
  public PackedScene Mesh;

  [Export]
  public Animation[] Animations;

  [Export]
  public ComponentType Component;
}
