using Godot;

class BaseAttack : IBehavior
{
  BaseNpcActor actor;

  public BaseAttack(BaseNpcActor actor)
  {
    this.actor = actor;
  }

  public void Finish() { }

  public void Handler(double delta) { }

  public void SetData(Variant data) { }

  public void Start()
  {
    GD.Print("Start attack");
  }
}
