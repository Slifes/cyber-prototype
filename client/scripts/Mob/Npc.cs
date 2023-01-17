using Godot;

using System.Collections.Generic;

enum NpcState
{
	Idle,
	Walking,
	Steering,
	Attacking,
	Died 
}

partial class Npc: BodyActor
{
	NpcState state = NpcState.Idle;

	IBehavior behavior;

	Dictionary<NpcState, IBehavior> behaviors;

	public Area3D AgressiveArea { get; set; }

	public Area3D AttackArea { get; set; }

	public Node3D Target {get;set;}

	public AnimationPlayer Animation {get;set;}

	public override void _Ready()
	{
		AgressiveArea = GetNode<Area3D>("AgressiveArea");
		AttackArea = GetNode<Area3D>("AttackArea");
		Animation = GetNode<AnimationPlayer>("AnimationPlayer");

		behaviors = new()
		{
			{NpcState.Steering, new BasedContextSteering(this)},
			{NpcState.Walking, new BaseMovement(this)},
			{NpcState.Attacking, new BaseAttack(this)}
		};

		ChangeState(NpcState.Walking);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (behavior != null)
		{
			behavior.Handler(delta);
		}
	}

	public void ChangeState(NpcState state)
	{
		GD.Print("New state: ", state);

		if (behavior != null)
		{
			behavior.Finish();
		}

		behavior = behaviors[state];

		behavior.Start();
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void MoveToTarget(Vector3 position, Vector3 velocity)
	{
		this.LinearVelocity = velocity;
		GlobalPosition = position;
		//state = MobState.Walking;
	}
}
