using Godot;
using System.Collections.Generic;

partial class Player : CharacterActor
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

  AnimationPlayer animationPlayer;

  List<Skill> skills; 

  bool mouseCameraPressed = false;

  PlayerState state;

  PackedScene DamageText = ResourceLoader.Load<PackedScene>("res://effects/Damage.tscn");

  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

  [Signal]
  public delegate void HealthStatusChangedEventHandler(int currentHP, int currentSP, int maxHP, int maxSP);

  public override void _Ready()
  {
	base._Ready();

	animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

	skills = LoadSkills();

	body = GetNode<Node3D>("Body");
	camera = GetNode<Node3D>("Camera");

	camera3d = camera.GetNode<Camera3D>("Camera3D");
	camera3d.Current = IsMultiplayerAuthority();

	if (InitialPosition != Vector3.Zero)
	{
	  LastUpdateTime = ServerBridge.Now();
	  UpdatePosition(InitialPosition);
	}

	GD.Print("New player: ", Name);

	if (IsMultiplayerAuthority())
	{
	  GD.Print("Authority: ", IsMultiplayerAuthority());
	  setPlayerToStats();
	}
	
	UpdateStats();
  }

  void setPlayerToStats()
  {
	var stats = GetNode<Stats>("/root/Stats");

	stats.setPlayer(this);
  }

  public override void _PhysicsProcess(double delta)
  {
	if (!IsMultiplayerAuthority())
	{
	  _ServerUpdatePosition((float)delta);
	}
	else
	{
	  _AuthorityController(delta);
	}
  }

  public override void TakeDamage(int damage)
  {
	base.TakeDamage(damage);

	camera3d.Call("add_trauma", 0.15f);

	UpdateStats();
	SpawnDamageText(damage);
  }

  protected void SpawnDamageText(int damage)
  {
	var damageText = DamageText.Instantiate<Node3D>();

	AddChild(damageText);

	damageText.Call("run", damage);
  }

  protected void UpdateStats()
  {
	EmitSignal(nameof(HealthStatusChanged), currentHP, currentSP, maxHP, maxSP);
  }

  public Vector3 GetActorRotation()
  {
	return body.Rotation;
  }

  public void SetActorRotation(Vector3 rotation)
  {
	body.Rotation = rotation;
  }

  #region send_movement
  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovement(Variant position, Variant yaw) { }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovementStopped(Variant position, Variant yaw) { }
  #endregion
}
