using Godot;

public partial class HealthStats : Control
{
	ColorRect maxHPBar;

	ColorRect currentHPBar;

	int currentHP = 100;

	int maxHP = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		maxHPBar = GetNode<ColorRect>("HP");
		currentHPBar = maxHPBar.GetNode<ColorRect>("Current");
	}

	public void SetCurrentHP(int currentHP, int maxHP)
	{
		this.currentHP = currentHP;
		this.maxHP = maxHP;

		currentHPBar.Size = new Vector2(maxHPBar.Size.x * ((float)currentHP / (float)maxHP), currentHPBar.Size.y);
	}
}
