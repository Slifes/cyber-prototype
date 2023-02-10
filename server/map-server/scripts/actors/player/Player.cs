using Godot;
using System.Collections.Generic;

partial class Player : CharacterActor
{
  [Signal]
  public delegate void EquipmentChangedEventHandler(Variant slot, Variant id);

  [Signal]
  public delegate void PickUpItemEventHandler(Variant itemId);

  [Signal]
  public delegate void MoneyValueChangedEventHandler(Variant value);

  [Signal]
  public delegate void MovementStartEventHandler(Variant position, Variant yaw);

  [Signal]
  public delegate void MovementStopEventHandler(Variant position, Variant yaw);

  Money money;

  SkillHandler skillHandler;

  Ghosting ghosting;

  Inventory inventory;

  Equipment equipment;

  AnimationPlayer animationPlayer;

  public Ghosting Ghost { get { return ghosting; } }

  public Inventory Inv { get { return inventory; } }

  public Equipment Equip { get { return equipment; } }

  public Money Zeny { get { return money; } }

  public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

  public override void _Ready()
  {
    base._Ready();

    SetMultiplayerAuthority(_actorId);

    ghosting = new Ghosting(this);
    skillHandler = new SkillHandler(this);
    money = new Money(this);
    inventory = new Inventory(this);
    equipment = new Equipment(this);
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

    ServerBridge.Instance.SendActorTookDamage(ghosting.NearestPlayers, this, damage);
  }

  public List<int> GetNearestPlayers()
  {
    return ghosting.NearestPlayers;
  }

  private void Move(Vector2 position, float rotation)
  {
    GlobalPosition = new Vector3(position.X, GlobalPosition.Y, position.Y);
    Rotation = new Vector3(0, rotation, 0);
  }
}
