using Godot;
using System.Collections.Generic;

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

  List<BaseShard> zones;

  public Inventory Inv { get { return inventory; } }

  public Equipment Equip { get { return equipment; } }

  public Money Zeny { get { return money; } }

  public override void _Ready()
  {
	base._Ready();

	SetMultiplayerAuthority(GetActorId());

	money = new Money(this);
	inventory = new Inventory(this);
	equipment = new Equipment(this);

	zones = new();
  }

  public void SendPacketToZone(string name, params Variant[] args)
  {
	foreach(var zone in zones)
	{
	  zone.Rpc(name, args);
	}
  }

  public void AddZone(BaseShard zone)
  {
	if (!zones.Contains(zone))
	{
	  zones.Add(zone);
	}
  }

  public void RemoveZone(BaseShard zone)
  {
	zones.Remove(zone);
  }
}
