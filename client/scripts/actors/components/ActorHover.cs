using Godot;

class ActorHover : IComponent
{
  static PackedScene HoverScene = ResourceLoader.Load<PackedScene>("res://components/ActorHover.tscn");

  Label3D Name;

  Node3D Components;

  public ActorHover(CharacterActor actor)
  {
    var node = HoverScene.Instantiate();

    actor.AddChild(node);

    Components = node.GetNode<Node3D>("Components");

    Name = Components.GetNode<Label3D>("Name");

    var area = node.GetNode<Area3D>("Area3D");

    area.MouseEntered += MouseEntered;
    area.MouseExited += MouseExited;

    Name.Text = actor.Name;
  }

  void MouseEntered()
  {
    Components.Visible = true;
  }

  void MouseExited()
  {
    Components.Visible = false;
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
