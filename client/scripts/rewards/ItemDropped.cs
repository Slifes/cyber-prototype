using Godot;

partial class ItemDropped : RigidBody3D
{
  [Export]
  public Item item;

  Sprite3D sprite;

  Area3D area;

  double maxLifeTime = 10;

  double lifeTime = 0;

  public override void _Ready()
  {
    base._Ready();

    sprite = GetNode<Sprite3D>("Sprite3D");
    area = GetNode<Area3D>("Area3D");

    sprite.Modulate = item.Icon;

    CallDeferred("RandomApplyForce");

    area.InputEvent += OnInputEvent;
    area.MouseEntered += OnMouseEntered;
    area.MouseExited += OnMouseLeft;
  }

  public override void _Input(InputEvent @event)
  {
    if (Input.IsActionJustPressed("attack"))
    {
      RandomApplyForce();
    }
  }

  void RandomApplyForce()
  {
    var rangeX = GD.RandRange(-0.2f, 0.2f);
    var rangeY = GD.RandRange(-0.2f, 0.2f);
    var direction = new Vector3((float)rangeX, 3f, (float)rangeY);
    GD.Print("Apply force: ", direction);
    ApplyForce(direction, GlobalPosition);
  }

  void OnInputEvent(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shape_idx)
  {
    if (@event.IsPressed())
    {
      GD.Print("Pick UP");
      NetworkManager.Instance.SendPacket(new Packets.Client.PlayerPickUpItem
      {
        dropID = int.Parse(Name)
      });

      OnMouseLeft();
    }
  }

  void OnMouseEntered()
  {
    Input.SetDefaultCursorShape(Input.CursorShape.PointingHand);
  }

  void OnMouseLeft()
  {
    Input.SetDefaultCursorShape(Input.CursorShape.Arrow);
  }

  public override void _Process(double delta)
  {
    lifeTime += delta;

    if (lifeTime > maxLifeTime)
    {
      QueueFree();
    }
  }
}
