using Godot;

class BaseAttack : IBehavior
{
  Npc actor;

  Area3D HurtBox;

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
      actor.Animation.Play("Attack");
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
    HurtBox = actor.GetNode<Area3D>("HurtBox");

    HurtBox.BodyEntered += HitTarget; 

    actor.AttackArea.BodyExited += BodyExited;

    LastOrigin = actor.Target.GlobalPosition;
  }

  private void HitTarget(Node3D target)
  {
    GD.Print("Damaged");
  
    IActor actor = (IActor)target;

    actor.TakeDamage(5);
  }

  private void BodyExited(Node3D body)
  {
    actor.ChangeState(NpcState.Steering);
  }
}