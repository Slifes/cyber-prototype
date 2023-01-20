using Godot;

using System;
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

	private Area3D AABB;

  private Area3D HitBox;

  public Skill CurrentSkill;

	ServerBridge serverBridge;

	List<int> nearest;

	public override void _Ready()
	{
		onActorReady();

		AABB = GetNode<Area3D>("AABB");
		AABB.BodyEntered += Area_BodyEntered;
		AABB.BodyExited += Area_BodyExited;

	HitBox = GetNode<Area3D>("HitBox");
	HitBox.BodyEntered += HitBodyEntered;
	
		serverBridge = GetNode<ServerBridge>("/root/World/Server");

		nearest = new();

		AgressiveArea = GetNode<Area3D>("AgressiveArea");
		AttackArea = GetNode<Area3D>("AttackArea");
		Animation = GetNode<AnimationPlayer>("AnimationPlayer");

		behaviors = new()
		{
			{NpcState.Walking, new BaseMovement(this)},
			{NpcState.Steering, new BasedContextSteering(this)},
			{NpcState.Attacking, new BaseAttack(this)}
		};

		ChangeState(NpcState.Walking);
	}

  private void HitBodyEntered(Node3D body)
  {
	GD.Print("Hitted Player");
	IActor actor = (IActor)body;

	  actor.TakeDamage(5);
  }

	private void Area_BodyExited(Node3D body)
	{
		IActor actor = (IActor)body;

		if (actor.GetActorType() == ActorType.Player && nearest.Contains(actor.GetActorId()))
		{
			nearest.Remove(actor.GetActorId());
		}
	}

	private void Area_BodyEntered(Node3D body)
	{
		IActor actor = (IActor)body;

		if (actor.GetActorType() == ActorType.Player)
		{
			nearest.Add(actor.GetActorId());
		}
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

		this.state = state;

		behavior.Start();

		serverBridge.SendNpcChangeState(nearest, Name, (int)state, GlobalPosition, RotationDegrees.y, behavior.GetData());
	}

	public void UpdateNPCState()
	{
		serverBridge.SendNpcUpdateState(nearest, Name, (int)state, GlobalPosition, RotationDegrees.y, behavior.GetData());
	}

  public void ExecuteSkill(int skillId)
  {
	Animation.Play(String.Format("Skills/{0}", skillId));

	serverBridge.SendSkillExecutedTo(nearest, this, skillId);
  }

	public override Variant GetData()
  {
		var data = new Godot.Collections.Array<Variant>()
		{
			0,
			currentHP,
			currentSP,
			maxHP,
			maxSP,
			(int)state,
			behavior.GetData(),
		};

		return data;
  }
}
