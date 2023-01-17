using Godot;

partial class Kirt : Npc
{
	AnimationPlayer anim;

	public override void _Ready()
	{
		base._Ready();
	
		anim = (AnimationPlayer)GetNode("AnimationPlayer");
	}
}
