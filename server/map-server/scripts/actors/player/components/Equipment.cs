using Godot;
using System.Collections.Generic;

public enum EquipmentSlot
{
  Head,
  RightHand,
  LeftHand,
}

class ActorEquipment
{
  Player actor;

  public Dictionary<EquipmentSlot, Item> slots;

  public ActorEquipment(Player actor)
  {
    slots = new();

    this.actor = actor;
  }

  public void Apply(int itemId)
  {
    var item = ItemManager.Instance.Get(itemId);

    if (item == null || item is not Equipment) return;

    var equip = (Equipment)item;

    actor.SendPacketToZone("StatisticChanged", equip.ID, equip.Attributes);
  }

  public void Remove(int itemId)
  {

  }
}
