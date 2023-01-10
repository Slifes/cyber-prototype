using Godot;

partial class Kirt : Mob
{
	AnimationPlayer anim;

	public override void _Ready()
	{
		base._Ready();

		_type = ActorType.Npc;

		actorReference = 0;
	}
}
