using Godot;

partial class Kirt : MobActor
{
	AnimationPlayer anim;

	public override void _Ready()
	{
		base._Ready();

		_type = ActorType.Npc;

		anim = (AnimationPlayer)GetNode("AnimationPlayer");
	}
}
