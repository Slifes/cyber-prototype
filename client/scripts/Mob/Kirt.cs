using Godot;
using System;

partial class Kirt : Actor
{
	enum State
	{
		Idle,
		Walking,
		Attacking
	}

	public const float Speed = 10.0f;

	AnimationPlayer anim;

	Vector3 targetLocation;

	State state;

	public override void _Ready()
	{
		base._Ready();

		_type = ActorType.Npc;

		state = State.Idle;

		anim = (AnimationPlayer)GetNode("AnimationPlayer");

		targetLocation = Vector3.Zero;
	}

	private void HandleIdle(double delta)
	{

	}

	private void HandleWalking(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		if (state == State.Idle)
		{
			HandleIdle(delta);
		} else
		{
			HandleWalking(delta);
		}
	}
}
