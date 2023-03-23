using Godot;
using Godot.Collections;

partial class BaseEnemy : BaseNPC
{
  [Export]
  public Array<Skill> skills;

  [Export]
  public Array<int> itemsId;

  [Export]
  public int MoneyMax;

  private Behavior behavior;

  private ActorState state;

  BattleStats battleStats;

  public override void _Ready()
  {
    base._Ready();

    behavior = new AgressiveBehavior(this);

    state = ActorState.Idle;

    Array<int> ids = new();

    battleStats = new(this);

    foreach (var id in skills)
    {
      ids.Add(id.ID);
    }

    EmitSignal(ZoneActor.SignalName.SkillList, ids);
  }

  public override void _PhysicsProcess(double delta)
  {
    behavior.Update(delta);
  }

  public override void TakeDamage(int actorId, int damage)
  {
    base.TakeDamage(actorId, damage);

    battleStats.ComputeDamage(actorId, damage);

    if (currentHP <= 0)
    {
      behavior.ChangeState(AIState.Died);
    }
  }

  public void SendDead()
  {
    var actorId = battleStats.GetActorByMaxDamage();

    Zone.SendActorDead(GetActorID(), GetActorType());
    // Zone.SendActorDrop(GetActorID(), GetActorType(), actorId, MoneyMax, itemsId);
  }

  public void ClearBattleStats()
  {
    battleStats.ClearDamage();
  }
}
