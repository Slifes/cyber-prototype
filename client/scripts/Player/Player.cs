using Godot;
using System;

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

	MultiplayerSynchronizer synchronizer;

	PlayerNetwork network;

	Node3D camera;

	Camera3D camera3d;

	MeshInstance3D mesh;

	bool mouseCameraPressed = false;

	PlayerState state;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = 9.9f;// ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		GD.Print("NEw player: ", Name);

		network = (PlayerNetwork)GetNode("Network");

		synchronizer = (MultiplayerSynchronizer)GetNode("Network/MultiplayerSynchronizer");
		synchronizer.SetMultiplayerAuthority(Int32.Parse(Name));

		mesh = (MeshInstance3D)GetNode("MeshInstance3D");
		camera = (Node3D)GetNode("Camera");

		camera3d = (Camera3D)camera.GetNode("Camera3D");

		camera3d.Current = synchronizer.IsMultiplayerAuthority();
		GD.Print("Camera: ", synchronizer.IsMultiplayerAuthority());
	}

	[RPC]
	public void Hello()
	{
		GD.Print("Hello: ", Multiplayer.GetRemoteSenderId());

		RpcId(1, "Pong");
	}

	[RPC]
	public void Pong() { }

	public override void _PhysicsProcess(double delta)
	{
		if (!synchronizer.IsMultiplayerAuthority())
		{
			GlobalPosition = network.position;
		}
		else
		{
			_AuthorityController(delta);
		}
	}
}
