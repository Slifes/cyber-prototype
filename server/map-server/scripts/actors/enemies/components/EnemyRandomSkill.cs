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

    var skill = actor.skills[i];

    var instance = skill.Scene.Instantiate<Node3D>();

    instance.Rotation = actor.Rotation;

    SkillNode.Spawn((AreaSkillBase)instance);

    instance.CallDeferred("set_global_position", actor.GlobalPosition);

    ServerBridge.Instance.SendSkillExecutedTo(actor.GetPlayersId(), actor, skill.ID);
  }
}
