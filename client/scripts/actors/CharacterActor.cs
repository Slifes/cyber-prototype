using System;
using Godot;

partial class CharacterActor : CharacterBody3D, IActor
{
  [Signal]
  public delegate void HealthStatusChangedEventHandler(int currentHP, int currentSP, int maxHP, int maxSP);

  [Signal]
  public delegate void TakeDamageEventHandler(int damage, int currentHP, int maxHP);

  [Signal]
  public delegate void ExecuteSkillEventHandler(Variant Id);

  protected int _actorId;

  protected ActorType _type;

  protected IComponent[] components;

  protected int currentHP;

  protected int currentSP;

  protected int maxHP;

  protected int maxSP;

  public void onActorReady()
  {
    _actorId = Int32.Parse(Name);

    maxHP = 100;
    maxSP = 100;

    currentHP = maxHP;
    currentSP = maxSP;
  }

  public override void _Ready()
  {
    onActorReady();
  }

  public int GetActorId()
  {
    return _actorId;
  }

  public int GetCurrentHP()
  {
    return currentHP;
  }

  public int GetCurrentSP()
  {
    return currentSP;
  }

  public int GetMaxHP()
  {
    return maxHP;
  }

  public int GetMaxSP()
  {
    return maxSP;
  }

  public virtual void SetServerData(Variant data)
  {
    var dataArray = data.AsGodotArray<Variant>();

    currentHP = (int)dataArray[0];
    currentSP = (int)dataArray[1];
    maxHP = (int)dataArray[2];
    maxSP = (int)dataArray[3];
  }

  public ActorType GetActorType()
  {
    return ActorType.Player;
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    foreach (var component in components)
    {
      component.InputHandler(@event);
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    foreach (var component in components)
    {
      component.Update((float)delta);
    }
  }

  public IComponent GetComponent<T>()
  {
    foreach (var component in components)
    {
      if (component is T)
      {
        return component;
      }
    }

    return null;
  }
}
