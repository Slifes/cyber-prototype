using Godot;

partial class Bullet : PhysicalAttack
{
  IActor owner;

  public override void _Ready()
  {
    GetNode<Area3D>("Area3D").BodyEntered += OnBodyEntered;
  }

  void OnBodyEntered(Node3D body)
  {
    var actor = (IActor)body;

    if (actor.GetActorId() != owner.GetActorId())
    {
      actor.TakeDamage(10);
      QueueFree();
    }
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
