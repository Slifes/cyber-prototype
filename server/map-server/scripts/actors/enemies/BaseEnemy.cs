using Godot;
using Godot.Collections;

partial class BaseEnemy : BaseNPC
{
  [Export]
  public Array<Skill> skills;

  private Behavior behavior;

  private ActorState state;

  Ghosting ghosting;

  public override void _Ready()
  {
    behavior = new AgressiveBehavior(this);

    state = ActorState.Idle;

    ghosting = new Ghosting(this);
  }

  public override void _PhysicsProcess(double delta)
  {
    behavior.Update(delta);
  }

  public override Variant GetData()
  {
    var data = new Godot.Collections.Array<Variant>()
    {
      ID,
      currentHP,
      maxHP,
    (int)state,
      behavior.GetData(),
    };

    return data;
  }

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);

    if (currentHP <= 0)
    {
      // this.ChangeState(AIState.Died);
    }

  }
}
