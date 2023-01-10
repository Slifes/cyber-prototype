using System;
using Godot;
using Godot.Collections;

partial class Player: Actor
{
	enum State
	{
		Idle,
		Walk
	}

	Area3D aabb;

	ServerBridge serverBridge;

	Node3D body;

	Node3D skillNode;

	State state;

	float Speed = 20.0f;

	Array<int> nearestPlayers;

	Array<int> nearest;

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

	public Array<int> GetNearestPlayers()
	{
		return nearestPlayers;
	}

	public override void _Ready()
	{
		base._Ready();

		_type = ActorType.Player;

		SetMultiplayerAuthority(_actorId);

		body = GetNode<Node3D>("Body");
		skillNode = GetNode<Node3D>("Body/Skill");

		serverBridge = GetNode<ServerBridge>("../../Server");

		aabb = GetNode<Area3D>("AABB");
		aabb.BodyEntered += OnBodyEntered;
		aabb.BodyExited += OnBodyExited;

		nearest = new();

		nearestPlayers = new();
	}

	private void OnBodyEntered(Node3D body)
	{
		var actor = (Actor)body;

		GD.Print("Body: ", body.Name);
		GD.Print("ActorId: ", actor.ActorID);

		if (body.Name != Name && !nearest.Contains(actor.ActorID))
		{
			nearest.Add(actor.ActorID);

			if (actor.IsPlayer())
			{
				nearestPlayers.Add(actor.ActorID);
			}

			serverBridge.SendActorEnteredZone(ActorID, actor);
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (body == null)
		{
			return;
		}

		var actor = (Actor)body;
		
		if (body.Name != Name && nearest.Contains(actor.ActorID))
		{
			nearest.Remove(actor.ActorID);

			if (actor.IsPlayer())
			{
				nearestPlayers.Remove(actor.ActorID);
			}

			serverBridge.SendActorExitedZone(ActorID, actor);
		}
	}

	private void Move(Vector2 position, float rotation, int nextState)
	{
		GlobalPosition = new Vector3(position.x, GlobalPosition.y, position.y);
		ActorRotation = new Vector3(0, rotation, 0);
		state = (State)nextState;
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SendMovement(Variant position, Variant yaw)
	{
		Move((Vector2)position, (float)yaw, (int)State.Walk);

		serverBridge.SendServerMovement(this, GlobalPosition, (float)yaw);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SendMovementStopped(Variant position, Variant yaw)
	{
		Move((Vector2)position, (float)yaw, (int)State.Idle);

		serverBridge.SendServerMovementStopped(this, GlobalPosition, (float)yaw);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ServerCurrentStats(Variant hp, Variant maxHp, Variant sp, Variant maxSp) { }

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
		}
	}
}
