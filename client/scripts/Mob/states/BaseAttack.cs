using Godot;

class BaseAttack : IBehavior
{
  Npc actor;
  
  public BaseAttack(Npc actor)
  {
    this.actor = actor;
  }

  public void Finish()
  {
  }

  public void Handler(double delta)
  {
    // var offset = (actor.Target.GlobalPosition - LastOrigin) * 2f * (float)delta;
    // LastOrigin = LastOrigin + offset;

    // actor.LookAt(LastOrigin, Vector3.Up);
  }

  public void SetData(Variant data)
  {

  }

  public void Start()
  {
    GD.Print("Start attack");
  }
}