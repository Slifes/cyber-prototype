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
  public const float Speed = 1.0f;

  PlayerState _state = PlayerState.Idle;

  IPlayerComponent[] components;

  Node3D _body;

  AnimationPlayer _animationPlayer;

  public PlayerState State { get { return _state; } }

  public Node3D Body { get { return _body; } }

  public AnimationPlayer Animation { get { return _animationPlayer; } }

  [Signal]
  public delegate void HealthStatusChangedEventHandler(int currentHP, int currentSP, int maxHP, int maxSP);

  [Signal]
  public delegate void TookDamageEventHandler(int damage);

  public override void _Ready()
  {
    base._Ready();

    _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

    skills = LoadSkills(new List<int>() { 0, 1, 2 });

    _body = GetNode<Node3D>("Body");

    GD.Print("New player: ", Name);

    MountPlayerComponents();
  }

  void MountPlayerComponents()
  {
    if (!IsMultiplayerAuthority())
    {
      SetProcessUnhandledInput(false);
      components = new IPlayerComponent[1]
      {
        new PredictMovement(this)
      };
    }
    else
    {
      components = new IPlayerComponent[4]
      {
        new CameraController(this),
        new MovementController(this),
        new MovementNetwork(this),
        new UIComponent(this)
      };
    }
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    foreach (var component in components)
    {
      component.InputHandler(@event);
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    foreach (var component in components)
    {
      component.Update((float)delta);
    }
  }

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);

    EmitSignal(nameof(TookDamage), damage);

    UpdateStats();
  }

  protected void UpdateStats()
  {
    EmitSignal(nameof(HealthStatusChanged), currentHP, currentSP, maxHP, maxSP);
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

}
