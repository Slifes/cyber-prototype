using Godot;
using Godot.Collections;

partial class PlayerZone : ZoneActor
{
  [Signal]
  public delegate void StatisticStateEventHandler(Dictionary<int, int> data);

  Ghosting ghosting;

  Statistic statistic;

  public override void _Ready()
  {
    base._Ready();

    ghosting = new Ghosting(this);

    statistic = new Statistic(this);
  }

  public override void _PhysicsProcess(double delta)
  {
    // testTimePhysics += (float)delta;

    // Vector3 velocity = Velocity;

    // if (!IsOnFloor())
    // {
    //   RunPhysics = true;
    //   velocity.Y -= gravity * (float)delta;
    // }

    // PhysicsTestMotionParameters3D t = new();
    // t.From = this.GlobalTransform;
    // t.Motion = velocity;

    // PhysicsTestMotionResult3D result = new();

    // bool hasCollided = PhysicsServer3D.BodyTestMotion(this.GetRid(), t, result);

    // if (hasCollided)
    // {
    //   velocity = velocity.Slide(result.GetCollisionNormal());
    // }

    // if (RunPhysics || testTimePhysics >= 0.5f)
    // {
    //   Position += velocity;

    //   Velocity = velocity;

    //   testTimePhysics = 0;
    // }
  }
}
