using System;
using System.Linq;
using Godot;
using Godot.Collections;

partial class Player: CharacterBody3D
{
	enum State
	{
		Idle,
		Walk
	}

	private int _id = 0;

	public bool Changed { get; set; }

	public int ActorID { get { return _id; } }

	Area3D aabb;

	WorldState worldState;

	Node3D body;

	Node3D skillNode;

	State state;

	float LastTickMovement;

	float Speed = 20.0f;

	public float gravity = 9.9f;// ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public Vector3 ActorRotation {
		get {
			return body.Rotation;
		}
		set
		{
			body.Rotation = value;
		}
	}

	Array<int> nearest;

	public Array<int> GetNearestPlayers()
	{
		return nearest;
	}

	public override void _Ready()
	{
		_id = Int32.Parse(Name);

		SetMultiplayerAuthority(_id);

		body = GetNode<Node3D>("Body");
		skillNode = GetNode<Node3D>("Body/Skill");

		worldState = GetNode<WorldState>("../../WorldState");

		aabb = GetNode<Area3D>("AABB");
		aabb.BodyEntered += OnBodyEntered;
		aabb.BodyExited += OnBodyExited;

		nearest = new();
	}

	private void OnBodyEntered(Node3D body)
	{
		var actor = ((Player)body);

		if (body.Name != Name && !nearest.Contains(actor.ActorID))
		{
			nearest.Add(actor.ActorID);

			worldState.CallDeferred("SendActorEnteredZone", ActorID, body.Name, (Variant)body.GlobalPosition);
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body == null)
		{
			return;
		}

		var actor = ((Player)body);
		
		if (body.Name != Name && nearest.Contains(actor.ActorID))
		{
			nearest.Remove(actor.ActorID);

			worldState.CallDeferred("SendActorExitedZone", int.Parse(Name), body.Name);
		}
	}

	private void Move(Vector2 position, float rotation, int nextState)
	{
		double now = WorldState.Now();

		GlobalPosition = new Vector3(position.x, GlobalPosition.y, position.y);
		ActorRotation = new Vector3(0, rotation, 0);
		LastTickMovement = (float)now;
		state = (State)nextState;

		Changed = true;
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SendMovement(Variant position, Variant yaw)
	{
		Move((Vector2)position, (float)yaw, (int)State.Walk);

		worldState.SendServerMovement(this, GlobalPosition, (float)yaw);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SendMovementStopped(Variant position, Variant yaw)
	{
		Move((Vector2)position, (float)yaw, (int)State.Idle);

		worldState.SendServerMovementStopped(this, GlobalPosition, (float)yaw);
	}

	public void RunSkill(Variant id)
	{
		var p = ResourceLoader.Load<PackedScene>("res://skills/normal_attack.tscn");

		var d = p.Instantiate();

		skillNode.AddChild(d);

		d.GetNode<AnimationPlayer>("Animation").Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.y -= gravity * (float)delta;

			Velocity = velocity;

			MoveAndSlide();
		} else
		{
			Velocity = Vector3.Zero;
		}
	}
}
