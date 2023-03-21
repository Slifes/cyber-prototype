using Godot;
using Godot.Collections;

partial class BaseEnemy : BaseNPC
{
  [Export]
  public Array<Skill> skills;

  [Export]
  public Array<Item> items;

  [Export]
  public int MoneyMax;

  private Behavior behavior;

  private ActorState state;

  public override void _Ready()
  {
    base._Ready();

    behavior = new AgressiveBehavior(this);

    state = ActorState.Idle;

    Array<int> ids = new();

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

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);

    if (currentHP <= 0)
    {
      behavior.ChangeState(AIState.Died);
    }
  }

  public void SendDropitems()
  {
    var list = new Godot.Collections.Array();

    foreach (var item in items)
    {
      var obj = new Godot.Collections.Dictionary();

      obj.Add("item", item.ID);
      obj.Add("amount", 1);

      list.Add(obj);
    }

    Zone.SendActorDrop(GetActorID(), GetActorType(), MoneyMax, list);
  }
}
