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
    var i = Mathf.Abs(GD.RandRange(0, actor.skills.Count - 1));

  }
}
