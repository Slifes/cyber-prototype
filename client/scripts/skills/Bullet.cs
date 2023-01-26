using Godot;

partial class Bullet: Node3D, ISkillEffect
{
  IActor owner;

  public override void _Ready()
  {
	GetNode<Area3D>("Area3D").BodyEntered += OnBodyEntered;
  }

  void OnBodyEntered(Node3D body)
  {
	QueueFree();
  }

  public void SetOwner(IActor actor)
  {
	owner = actor;
  }

  public void SetEffectRotation(Vector3 rotation)
  {
	  GD.Print("Rotation: ", rotation);
	  this.Rotation = rotation;
  }
  
  public void SetEffectPosition(Vector3 position)
  {
	  this.GlobalPosition = position;
  }
}
