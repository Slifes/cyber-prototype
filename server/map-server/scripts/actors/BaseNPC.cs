using Godot;

partial class BaseNPC : CharacterActor
{
  [Export]
  public int ID;

  public override ActorType GetActorType()
  {
    return ActorType.Npc;
  }

  public override Variant GetData()
  {
    var data = new Godot.Collections.Array<Variant>()
    {
        ID,
        currentHP,
        currentSP,
        maxHP,
        maxSP,
    };

    return data;
  }
}
