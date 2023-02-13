class Money
{
  int value;

  public int Value { get { return value; } }

  public Money(IActor actor)
  {

  }

  public bool HasEnough(int requested)
  {
    return value >= requested;
  }

  public void Transfer(int amount)
  {
    value -= amount;
  }

  public void Receive(int amount)
  {
    value += amount;
  }
}
