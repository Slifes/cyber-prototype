using Godot;

public partial class HealthStats : Control
{
  ProgressBar hpBar;

  public override void _Ready()
  {
    hpBar = GetNode<ProgressBar>("Container/HP");
  }

  public void SetCurrentHP(int currentHP, int maxHP)
  {
    hpBar.MaxValue = maxHP;
    hpBar.Value = currentHP;
  }
}
