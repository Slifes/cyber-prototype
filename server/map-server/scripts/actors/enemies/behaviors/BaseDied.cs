using Godot;

class BaseDied : IBehavior
{
  IActor actor;

  float TimeUnspawn = 1;

  float currentTime = 0;

  public BaseDied(IActor actor)
  {
    this.actor = actor;
  }

  public void Finish()
  {
    ((Node)this.actor).QueueFree();
  }

  public Variant GetData()
  {
    return new Variant();
  }

  public void Handler(double delta)
  {
    currentTime += (float)delta;

    if (currentTime >= TimeUnspawn)
    {
      this.Finish();
    }
  }

  public void Start()
  {
    currentTime = 0;
  }
}
