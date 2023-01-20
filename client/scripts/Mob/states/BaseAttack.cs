using Godot;

class BaseAttack : IBehavior
{
  Npc actor;
  
  public BaseAttack(Npc actor)
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