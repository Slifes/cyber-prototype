using Godot;
using System.Collections.Generic;

partial class Player : SessionActor
{
  Money money;

  Inventory inventory;

  ActorEquipment equipment;

  List<Zone> zones;

  public Inventory Inv { get { return inventory; } }

  public ActorEquipment Equip { get { return equipment; } }

  public Money Zeny { get { return money; } }

  public override void _Ready()
  {
    base._Ready();

    SetMultiplayerAuthority(GetActorId());

    money = new Money(this);
    inventory = new Inventory(this);
    equipment = new ActorEquipment(this);

    zones = new();

    AddItems();
  }

  void AddItems()
  {
    inventory.Add(0, 2);
  }

  public void SendPacketToZone(string name, params Variant[] args)
  {
    zones.ForEach((zone) => zone.Rpc(name, args));
  }

  public void AddZone(Zone zone)
  {
    if (!zones.Contains(zone))
    {
      zones.Add(zone);
    }
  }

  public void RemoveZone(Zone zone)
  {
    zones.Remove(zone);
  }

  public Packets.Server.SMActorEnteredZone GetActorPacket()
  {
    return new Packets.Server.SMActorEnteredZone
    {
      ActorId = GetActorId(),
      ActorType = (int)ActorType.Player,
      Position = new float[3] { Position.X, Position.Y, Position.Z },
      Yaw = Rotation.Y,
      Data = GetData().AsByteArray()
    };
  }
}
