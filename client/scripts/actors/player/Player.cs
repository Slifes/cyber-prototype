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

  [Signal]
  public delegate void SvLoadSkillsEventHandler(Variant skillIds);

  [Signal]
  public delegate void VoiceReceivedEventHandler(byte[] data);

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

    MountComponents();
  }

  void MountComponents()
  {
    if (!IsMultiplayerAuthority())
    {
      SetProcessUnhandledInput(false);
      components = new IComponent[5]
      {
        new SkillController(this, new List<int>() { 0, 1, 2 }),
		//new PredictMovement(this),
		new MiniHPBar(this),
        new ActorHover(this),
        new EffectComponent(this),
    new SpeakerComponent(this)
      };
    }
    else
    {
      components = new IComponent[7]
      {
        new SkillController(this, new List<int>() { 0, 1, 2 }),
        new CameraController(this),
        new MovementController(this),
        new MovementNetwork(this),
        new EffectComponent(this),
        new UIComponent(this),
    new TalkerComponent(this)
      };

      LoadSkills();
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

    PlayerUI.SetSkills(controller.Skills);
  }

}
