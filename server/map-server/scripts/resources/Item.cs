using Godot;

partial class Item : Resource
{
  [Export]
  public int ID;

  [Export]
  public string Name;

  [Export]
  public ItemType Type;

  [Export]
  public EquipmentSlot EquipSlot;

  [Export]
  public int SkillId;
}
