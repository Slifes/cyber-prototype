using Godot;

partial class Damage : Node3D
{
  PackedScene DamageScene = ResourceLoader.Load<PackedScene>("res://effects/Damage.tscn");

  public void Spawn(IActor actor, int damage)
  {
	var damageText = DamageScene.Instantiate<Node3D>();
	var color = "#ff0002";

	if (!((Node)actor).IsMultiplayerAuthority())
	{
	  color = "#ffffff";
	}

	AddChild(damageText);

	damageText.GlobalPosition = ((Node3D)actor).GlobalPosition;


	damageText.Call("run", damage, color);
  }
}
