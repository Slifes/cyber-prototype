using Godot;
using Godot.Collections;

partial class ZoneActor : CharacterBody3D, IActorZone
{
  [Signal]
  public delegate void SkillListEventHandler(Array<int> skillsId);

  [Signal]
  public delegate void ExecuteSkillEventHandler(int skillId, Variant data);

  Ghosting ghosting;

  SkillHandler skillHandler;

  public override void _Ready()
  {
    ghosting = new Ghosting(this);

    skillHandler = new SkillHandler(this);
  }

  public int GetActorID()
  {
    return System.Int32.Parse(Name);
  }

  public virtual ActorType GetActorType()
  {
    return ActorType.Player;
  }

  public Vector3 GetActorPosition()
  {
    return Position;
  }

  public virtual void TakeDamage(int value)
  {
    Zone.SendActorDamage(this.GetActorID(), (int)this.GetActorType(), value);
  }

  public virtual Variant GetData()
  {
    return new Array<Variant>()
    {
      100,
      100,
      100,
      100
    };
  }
}
