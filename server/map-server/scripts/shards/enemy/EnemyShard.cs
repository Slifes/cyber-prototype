using Godot;

partial class EnemyShard : BaseShard
{
  [Rpc()]
  public void OnExecuteSkill(Variant actorId, Variant skillId)
  { }
}
