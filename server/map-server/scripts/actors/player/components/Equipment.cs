using Godot;
using System.Collections.Generic;

enum EquipmentSlot
{
  Head,
  RightHand,
  LeftHand,
}

class Equipment
{
  Player actor;

  public Dictionary<EquipmentSlot, Item> slots;

  public Equipment(Player actor)
  {

  }

  public void ApplyEquipment(Variant itemId)
  {

    // ServerBridge.Instance.UpdateEquipment(actor.GetActorId(), itemId);

    actor.EmitSignal(Player.SignalName.EquipmentChanged, itemId);
  }
}
