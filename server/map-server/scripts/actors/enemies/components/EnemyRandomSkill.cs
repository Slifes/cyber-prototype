using Godot;

class EnemyRandomSkill
{
  BaseEnemy actor;

  public EnemyRandomSkill(BaseEnemy actor)
  {
    this.actor = actor;
  }

  public void Execute()
  {
    var skill = actor.skills[Mathf.Abs(GD.RandRange(0, actor.skills.Count - 1))];

    GD.Print("Execute Random Skill: ", skill.ID);

    actor.EmitSignal(ZoneActor.SignalName.ExecuteSkill, skill.ID, new Variant());
  }
}
