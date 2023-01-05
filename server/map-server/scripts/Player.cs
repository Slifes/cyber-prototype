using System;
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

	public int ActorID { get { return _id; } }

	Area3D aabb;

	WorldState worldState;

	State state;

	float lastTickMovement;

	float Speed = 20.0f;

	public float gravity = 9.9f;// ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public Vector3 ActorRotation { get; set; }

	[Export]
	Dictionary<Variant, CharacterBody3D> nearest;

	public Dictionary<Variant, CharacterBody3D> GetNearestPlayers()
	{
		return nearest;
	}

	public override void _Ready()
	{
		_id = Int32.Parse(Name);

		SetMultiplayerAuthority(_id);

		worldState = GetNode<WorldState>("../../WorldState");

		aabb = GetNode<Area3D>("AABB");
		aabb.BodyEntered += OnBodyEntered;
		aabb.BodyExited += OnBodyExited;

		nearest = new();
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body.Name != Name && !nearest.ContainsKey(body.Name))
		{
			nearest.Add(body.Name, (CharacterBody3D)body);

			worldState.CallDeferred("SendActorEnteredZone", int.Parse(Name), body.Name, (Variant)body.GlobalPosition);
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body.Name != Name && nearest.ContainsKey(body.Name))
		{
			nearest.Remove(body.Name);

			worldState.CallDeferred("SendActorExitedZone", int.Parse(Name), body.Name);
		}
	}

	private void Move(Vector3 position, Vector3 rotation, int nextState)
	{
		double now = WorldState.Now();

		Vector3 currentPosition = GlobalPosition;

		// float distance = currentPosition.DistanceTo(position);

		/*if (state == State.Walk)
		{
			var delta = (float)now - lastTickMovement;

			var seconds = delta / 1000;

			GD.Print("PlayerID: ", ActorID);
			GD.Print("seconds delay: ", seconds);
			GD.Print("Seed seconds: ", Speed * seconds);
			GD.Print("Distance: ", distance);

			if (distance > Speed * 2 * seconds)
			{
				GD.Print("Cheating...");

				Multiplayer.MultiplayerPeer.DisconnectPeer(_id);

				return;
			}
		}*/

		GlobalPosition = position;// new Vector3(position.x, GlobalPosition.y, position.y);
		ActorRotation = rotation;
		lastTickMovement = (float)now;
		state = (State)nextState;
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void Moving(Variant position, Variant rotation)
	{
		CallDeferred("Move", (Vector3)position, (Vector3)rotation, (int)State.Walk);
	}


	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void MoveStopped(Variant position, Variant rotation)
	{
		CallDeferred("Move", (Vector3)position, (Vector3)rotation, (int)State.Idle);
	}

	public void RunSkill(Variant id)
	{
		var p = ResourceLoader.Load<PackedScene>("res://skills/normal_attack.tscn");

		var d = p.Instantiate();

		GetNode<Node3D>("Skill").AddChild(d);

		d.GetNode<AnimationPlayer>("Animation").Play();
	}

	/*	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			GD.Print("Is in floor");
			velocity.y -= gravity * (float)delta;
		}

		Velocity = velocity;

		MoveAndSlide();

		GD.Print("Player: ", Name);
		GD.Print("Floor: ", GlobalPosition.y);
	}*/
}
