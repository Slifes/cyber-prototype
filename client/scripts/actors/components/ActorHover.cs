using Godot;

class ActorHover : IComponent
{
  static PackedScene HoverScene = ResourceLoader.Load<PackedScene>("res://components/ActorHover.tscn");

  Decal hoverDecal;

  public ActorHover(CharacterActor actor)
  {
    var node = HoverScene.Instantiate();

    actor.AddChild(node);

    hoverDecal = node.GetNode<Decal>("HoverDecal");

    var area = node.GetNode<Area3D>("Area3D");

    area.MouseEntered += MouseEntered;
    area.MouseExited += MouseExited;
  }

  void MouseEntered()
  {
    hoverDecal.Visible = true;
  }

  void MouseExited()
  {
    hoverDecal.Visible = false;
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
