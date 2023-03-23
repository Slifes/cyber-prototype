using Godot;
using Godot.Collections;

partial class ZoneActor : CharacterBody3D, IActorZone
{
  [Signal]
  public delegate void SkillListEventHandler(Array<int> skillsId);

  [Signal]
  public delegate void ExecuteSkillEventHandler(int skillId, Variant data);

  protected SkillHandler skillHandler;

  public override void _Ready()
  {
    skillHandler = new SkillHandler(this);
  }

  public int GetActorID()
  {
    return int.Parse(Name);
  }

  public virtual ActorType GetActorType()
  {
    return ActorType.Player;
  }

  public Vector3 GetActorPosition()
  {
    return Position;
  }

  public virtual void TakeDamage(int actorId, int value)
  {
    Zone.SendActorEffect(this.GetActorID(), this.GetActorType(), EffectType.Damage, value);
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
