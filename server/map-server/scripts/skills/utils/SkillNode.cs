using Godot;

partial class SkillNode : Node3D
{
  private static SkillNode instance;

  public override void _Ready()
  {
    instance = this;
  }

  public static void Spawn(Node skill)
  {
    instance.CallDeferred("add_child", skill);
  }
}
