using Godot;
using Godot.Collections;

partial class BaseEnemy : BaseNPC
{
  [Export]
  public Array<Skill> skills;

  private Behavior behavior;

  public override void _Ready()
  {
    onActorReady();

    behavior = new AgressiveBehavior(this);
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
	  // currentHP,
	  // currentSP,
	  // maxHP,
	  // maxSP,
	  // (int)state,
	  behavior.GetData(),
    };

    return data;
  }

  // public override void TakeDamage(int damage)
  // {
  // base.TakeDamage(damage);

  // if (currentHP <= 0)
  // {
  //   // this.ChangeState(AIState.Died);
  // }

  // //ServerBridge.Instance.SendActorTookDamage(ghosting.NearestPlayers, this, damage);
  // }
}
