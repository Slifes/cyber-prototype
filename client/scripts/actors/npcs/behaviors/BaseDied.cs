using Godot;

class BaseDied : IBehavior
{
  BaseNpcActor actor;

  public BaseDied(BaseNpcActor actor)
  {
    this.actor = actor;
  }

  public void Finish() { }

  public void Handler(double delta) { }

  public void SetData(Variant data) { }

  public void Start()
  {
    actor.Animation.Stop(true);
    actor.Animation.Play("Die");
  }
}
