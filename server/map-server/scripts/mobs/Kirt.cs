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

	NavigationAgent3D agent;

	RayCast3D rayCast;

	Node3D target;

	AnimationPlayer anim;

	Random random;

	Vector3 targetLocation;

	State state;

	double LastIdleTimeChecked;

	public override void _Ready()
	{
		base._Ready();

		_type = ActorType.Npc;

		state = State.Idle;

		agent = GetNode<NavigationAgent3D>("NavigationAgent3D");

		random = new Random();

		targetLocation = Vector3.Zero;
	}

	private void HandleIdle(double delta)
	{

		GD.Print("handleidle");
		var now = Time.GetUnixTimeFromSystem();

		if ((now - LastIdleTimeChecked) > 2)
		{
			var index = random.Next(10);

			if (index % 2 == 0)
			{
				state = State.Walking;
			}

			LastIdleTimeChecked = now;
		}
	}

	private void HandleWalking(double delta)
	{
		GD.Print("handlewalking");
		if (targetLocation == Vector3.Zero)
		{
			var random = new RandomNumberGenerator();

			var mobX = random.Randf() * 4;
			var mobY = random.Randf() * 4;

			targetLocation = new Vector3(mobX, GlobalPosition.y, mobY);

			agent.TargetLocation = targetLocation;
		}

		GD.Print("TargetLocation: ", targetLocation);
		GD.Print("IsReachable: ", agent.IsTargetReachable());

		if (agent.IsTargetReachable())
		{
			if (agent.IsTargetReached())
			{
				state = State.Idle;
				targetLocation = Vector3.Zero;
			}
			else
			{
				var next = agent.GetNextLocation();

				var dir = GlobalPosition.DirectionTo(next);

				Velocity = dir * (float)delta * Speed;

				MoveAndSlide();
			}
		}
		else
		{
			targetLocation = Vector3.Zero;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (state == State.Idle)
		{
			HandleIdle(delta);
		}
		else
		{
			HandleWalking(delta);
		}
	}
}
