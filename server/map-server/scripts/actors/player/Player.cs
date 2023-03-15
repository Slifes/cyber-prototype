using Godot;
using System.Collections.Generic;

partial class Player : SessionActor
{
  Money money;

  Inventory inventory;

  ActorEquipment equipment;

  List<BaseShard> zones;

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
  }

  public void SendPacketToZone(string name, params Variant[] args)
  {
    foreach (var zone in zones)
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

  public Packets.Server.SMActorEnteredZone GetActorPacket()
  {
    return new Packets.Server.SMActorEnteredZone
    {
      ActorId = GetActorId(),
      ActorType = (int)ActorType.Player,
      Position = new float[3] { Position.X, Position.Y, Position.Z },
      Yaw = Rotation.Y
    };
  }
}
