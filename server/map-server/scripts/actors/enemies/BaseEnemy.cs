using Godot;

using System.Collections.Generic;

partial class BaseEnemy : BaseNPC
{
  [Export]
  public Godot.Collections.Array<Skill> skills;

  private Ghosting ghosting;

  private Behavior behavior;

  public override void _Ready()
  {
    onActorReady();

    ghosting = new Ghosting(this);

    behavior = new AgressiveBehavior(this);
  }

  public List<int> GetPlayersId()
  {
    return ghosting.NearestPlayers;
  }

  public override void _PhysicsProcess(double delta)
  {
    behavior.Update(delta);
  }

  public void ExecuteSkill(int skillId)
  {
    //ServerBridge.Instance.SendSkillExecutedTo(, this, skillId);
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

    ServerBridge.Instance.SendActorTookDamage(ghosting.NearestPlayers, this, damage);
  }
}
