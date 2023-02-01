using System;
using System.Collections.Generic;
using Godot;

partial class Player: CharacterActor
{
  enum State
  {
	Idle,
	Walk
  }

  Area3D aabb;

  AnimationPlayer animationPlayer;

  Area3D hitBox;

  State state;

  float Speed = 20.0f;

  private Skill currentSkill;

  List<Skill> skills;

  List<int> nearestPlayers;

  List<int> nearest;

  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

  public List<int> GetNearestPlayers()
  {
	return nearestPlayers;
  }

  public override void _Ready()
  {
	base._Ready();

	SetMultiplayerAuthority(_actorId);

	animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	hitBox = GetNode<Area3D>("HitBox");
	aabb = GetNode<Area3D>("AABB");

	aabb.BodyEntered += OnBodyEntered;
	aabb.BodyExited += OnBodyExited;
	hitBox.BodyEntered += OnHitBoxEntered;
	animationPlayer.AnimationFinished += OnSkillAnimationFinished;

	nearest = new();
	nearestPlayers = new();
	skills = new();
  }

  private void OnSkillAnimationFinished(StringName name)
  {
	currentSkill = null;
  }

  private void OnHitBoxEntered(Node3D body)
  {
	if (body.Name != Name){
	  GD.Print("Hitted NPC");
	  IActor actor = (IActor)body;

	  actor.TakeDamage(currentSkill.Damage);
	}
  }

  private void OnBodyEntered(Node3D body)
  {
	GD.Print(body);

	var actor = (IActor)body;

	GD.Print("Body: ", body.Name);
	GD.Print("ActorId: ", actor.GetActorId());

	if (body.Name != Name && !nearest.Contains(actor.GetActorId()))
	{
	  nearest.Add(actor.GetActorId());

	  if (actor.GetActorType() == ActorType.Player)
	  {
		nearestPlayers.Add(actor.GetActorId());
	  }

	  ServerBridge.Instance.SendActorEnteredZone(GetActorId(), actor);
	}
  }

  private void OnBodyExited(Node3D body)
  {
	if (body == null)
	{
	  return;
	}

	var actor = (IActor)body;
	
	if (body.Name != Name && nearest.Contains(actor.GetActorId()))
	{
	  nearest.Remove(actor.GetActorId());

	  if (actor.GetActorType() == ActorType.Player)
	  {
		nearestPlayers.Remove(actor.GetActorId());
	  }

	  ServerBridge.Instance.SendActorExitedZone(GetActorId(), actor);
	}
  }

  private void Move(Vector2 position, float rotation, int nextState)
  {
	GlobalPosition = new Vector3(position.X, GlobalPosition.Y, position.Y);
	Rotation = new Vector3(0, rotation, 0);
	state = (State)nextState;
  }

  public void LoadSkill(List<int> dbSkills)
  {
	  var animationLibrary = animationPlayer.GetAnimationLibrary("Skills");
	
		foreach(var id in dbSkills)
		{
			Skill skill = SkillManager.Instance.Get(id);

			if (skill != null)
			{
				skills.Add(skill);

				if (skill.Type == SkillType.Active && skill.animation != null)
				{
					animationLibrary.AddAnimation(skill.ID.ToString(), skill.animation);
				}
			}
		}
  }

  public void RunSkill(Variant id)
  {
		currentSkill = SkillManager.Instance.Get(id.AsInt32());

		if (currentSkill.animation != null)
		{
			animationPlayer.Play(String.Format("Skills/{0}", id.ToString()));
		}
		
		if (currentSkill.Effect != null)
		{
			Node skillEffect = currentSkill.Effect.Instantiate();
			ISkillEffect effect = (ISkillEffect)skillEffect;

			effect.SetOwner(this);

			GetNode("/root/World/Effects").AddChild(skillEffect);

			effect.SetEffectRotation(this.Rotation);
			effect.SetEffectPosition(this.GlobalPosition);
		}

  }

  public override void _PhysicsProcess(double delta)
  {
	Vector3 velocity = Velocity;

	// Add the gravity.
	if (!IsOnFloor())
	{
	  velocity.Y -= gravity * (float)delta;

	  Velocity = velocity;

	  MoveAndSlide();
	}
  }

  public override void TakeDamage(int damage)
  {
	base.TakeDamage(damage);

	ServerBridge.Instance.SendActorTookDamage(this.nearestPlayers, this, damage);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovement(Variant position, Variant yaw)
  {
	Move((Vector2)position, (float)yaw, (int)State.Walk);

	ServerBridge.Instance.SendServerMovement(this, GlobalPosition, (float)yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovementStopped(Variant position, Variant yaw)
  {
	Move((Vector2)position, (float)yaw, (int)State.Idle);

	ServerBridge.Instance.SendServerMovementStopped(this, GlobalPosition, (float)yaw);
  }

	
  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant id)
  {
	GD.Print("Received Request skill: ", GetActorId());

	ServerBridge.Instance.SendSkillExecutedTo(GetNearestPlayers(), this, (int)id);
  
	RunSkill(id);
  }

}
