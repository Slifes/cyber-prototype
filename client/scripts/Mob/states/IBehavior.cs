using Godot;

interface IBehavior
{
  void Start(Npc actor);

  void Handler(Npc actor, double delta);

  void Finish(Npc actor);
}