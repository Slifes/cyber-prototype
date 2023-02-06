using Godot;

class HitBox : IComponent
{
  IActor actor;

  public HitBox(Node actor)
  {
    var area = new Area3D();
    var collision = new CollisionShape3D();
    var box = new BoxShape3D();

    area.AddChild(collision);
    collision.Shape = box;

    actor.AddChild(area);
  }
}
