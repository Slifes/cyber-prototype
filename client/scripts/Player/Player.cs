using Godot;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{
	public const float Speed = 1.0f;
	public const float JumpVelocity = 4.5f;

	enum PlayerState
	{
		Idle,
		Walking
	}

	Vector2 mouseMoveCameraInitial = Vector2.Zero;

	public Vector3 InitialPosition = Vector3.Zero;

	MultiplayerSynchronizer synchronizer;

	Node3D camera;

	Camera3D camera3d;

	Node3D body;

	Node3D skillNode;

	Node3D networking;

	List<Node3D> skills; 

	bool mouseCameraPressed = false;

	PlayerState state;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 9.9f;// ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		body = GetNode<Node3D>("Body");
		camera = GetNode<Node3D>("Camera");

		camera3d = camera.GetNode<Camera3D>("Camera3D");
		camera3d.Current = IsMultiplayerAuthority();

		skillNode = GetNode<Node3D>("Body/Skill");

		// networking = GetNode<Node3D>("../../World");

		if (InitialPosition != Vector3.Zero)
		{
			LastUpdateTime = WorldState.Now();
			UpdatePosition(InitialPosition);
		}

		GD.Print("New player: ", Name);
		GD.Print("Authority: ", IsMultiplayerAuthority());

		ResourceLoader.LoadThreadedRequest("res://skills/normal_attack.tscn");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsMultiplayerAuthority())
		{
			_ServerUpdatePosition((float)delta);
		} else {
			_AuthorityController(delta);
		}
	}

	public Vector3 GetActorRotation()
	{
		return body.Rotation;
	}

	public void SetActorRotation(Vector3 rotation)
	{
		body.Rotation = rotation;
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SendMovement(Variant position, Variant yaw) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SendMovementStopped(Variant position, Variant yaw) { }
}
