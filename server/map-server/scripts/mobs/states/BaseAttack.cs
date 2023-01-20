using Godot;

class BaseAttack : IBehavior
{
  Npc actor;

  private float time;

  private float targetTime = 1;

  private Vector3 LastOrigin;

  public BaseAttack(Npc actor)
  {
    this.actor = actor;
  }

  public void Finish()
  {
    actor.AttackArea.BodyExited -= BodyExited;
  }

  public void Handler(double delta)
  {
    if(time >= targetTime){
      actor.ExecuteSkill(0);
      time = 0;
    } else {
      time += (float)delta;
    }

    var offset = (actor.Target.GlobalPosition - LastOrigin) * 2f * (float)delta;
    LastOrigin = LastOrigin + offset;

    actor.LookAt(LastOrigin, Vector3.Up);
  }

  public void Start()
  {
    actor.AttackArea.BodyExited += BodyExited;

    LastOrigin = actor.Target.GlobalPosition;
  }


  private void BodyExited(Node3D body)
  {
    actor.ChangeState(NpcState.Steering);
  }

  public Variant GetData()
  {
    return Vector3.Zero;
  }
}