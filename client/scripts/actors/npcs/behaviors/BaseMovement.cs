using Godot;

class BaseMovement : IBehavior
{
  Behavior actor;

  public BaseMovement(Behavior actor)
  {
    this.actor = actor;
  }

  public void Start()
  {
    // actor.Actor.Animation.Play("Walking");
  }

  public void Handler(double delta)
  {

  }

  public void Finish() { }

  public void SetData(Variant data) { }
}
