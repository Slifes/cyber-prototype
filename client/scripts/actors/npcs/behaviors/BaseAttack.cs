using Godot;

class BaseAttack : IBehavior
{
  Behavior actor;

  public BaseAttack(Behavior actor)
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
