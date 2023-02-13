using Godot;

partial class Player : SessionActor
{
  [Signal]
  public delegate void EquipmentChangedEventHandler(Variant slot, Variant id);

  [Signal]
  public delegate void PickUpItemEventHandler(Variant itemId);

  [Signal]
  public delegate void MoneyValueChangedEventHandler(Variant value);

  Money money;

  Inventory inventory;

  Equipment equipment;

  public Inventory Inv { get { return inventory; } }

  public Equipment Equip { get { return equipment; } }

  public Money Zeny { get { return money; } }

  public override void _Ready()
  {
    base._Ready();

    SetMultiplayerAuthority(_actorId);

    money = new Money(this);
    inventory = new Inventory(this);
    equipment = new Equipment(this);
  }
}
