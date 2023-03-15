using Godot;
using Godot.Collections;

partial class Equipment : Item
{
  [Export]
  public EquipmentSlot Slot;

  [Export]
  public Array<Attribute> Attributes;
}
