using Godot;
using System.Collections.Generic;

enum PlayerState
{
  Idle,
  Walking,
  Died,
}

partial class Player : CharacterActor
{
  [Signal]
  public delegate void SvStartMovementEventHandler(Variant position, Variant yaw);

  [Signal]
  public delegate void SvStopMovementEventHandler(Variant position, Variant yaw);

  public const float Speed = 1.0f;

  PlayerState _state = PlayerState.Idle;

  Node3D _body;

  AnimationPlayer _animationPlayer;

  public PlayerState State { get { return _state; } }

  public Node3D Body { get { return _body; } }

  public AnimationPlayer Animation { get { return _animationPlayer; } }

  public override void _Ready()
  {
    base._Ready();

    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

    _body = GetNode<Node3D>("Body");

    components = CreateComponents();

    LoadSkills();
  }

  IComponent[] CreateComponents()
  {
    if (!IsMultiplayerAuthority())
    {
      SetProcessUnhandledInput(false);
      return new IComponent[4]
      {
      new SkillController(this, new List<int>() { 0, 1, 2 }),
      new PredictMovement(this),
      new MiniHealth(this),
        new ActorHover(this),
      };
    }
    else
    {
      return new IComponent[5]
      {
      new SkillController(this, new List<int>() { 0, 1, 2 }),
      new CameraController(this),
      new MovementController(this),
      new MovementNetwork(this),
      new UIComponent(this)
      };
    }
  }

  public Vector3 GetBodyRotation()
  {
    return Body.Rotation;
  }

  public void SetBodyRotation(Vector3 rotation)
  {
    Body.Rotation = rotation;
  }

  public void ChangeState(PlayerState state)
  {
    this._state = state;
  }

  void LoadSkills()
  {
    SkillController controller = (SkillController)GetComponent<SkillController>();

    UIControl.SetSkills(controller.Skills);
  }

}
