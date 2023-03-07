using System;
using System.Collections.Generic;
using Godot;

partial class CharacterActor : CharacterBody3D, IActor
{
  [Signal]
  public delegate void ActorClickedEventHandler();

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

    if (!IsMultiplayerAuthority())
    {
      interpolate = new();
    }
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

  public virtual ActorType GetActorType()
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

    if (!IsMultiplayerAuthority())
    {
      InterpolateMove((float)delta);
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

  struct InterpolateMovement
  {
    public Vector3 Position;
    public float Yaw;
    public ulong Tick;
    public bool Stop;
  }

  List<InterpolateMovement> interpolate;

  void InterpolateMove(float delta)
  {
    var time = NetworkManager.Instance.ServerTick - 100;

    if (interpolate.Count > 1)
    {
      while (interpolate.Count > 2 && time > interpolate[1].Tick)
      {
        interpolate.RemoveAt(0);
      }

      var factor = Mathf.Clamp((float)(time - interpolate[0].Tick) / (float)(interpolate[1].Tick - interpolate[0].Tick), 0, 1);

      Position = interpolate[0].Position.Lerp(interpolate[1].Position, factor);
      Rotation = new Vector3(0, Mathf.Lerp(interpolate[0].Yaw, interpolate[1].Yaw, factor), 0);
    }

  }

  public void PushCommand(Packets.Server.IServerCommand command)
  {
    switch (command)
    {
      case Packets.Server.SMActorStartMove pck:
        interpolate.Add(new InterpolateMovement
        {
          Position = new Vector3(pck.Position[0], pck.Position[1], pck.Position[2]),
          Yaw = pck.Yaw,
          Tick = pck.Tick,
          Stop = false
        });
        break;
      case Packets.Server.SMActorStopMove pck:
        interpolate.Add(new InterpolateMovement
        {
          Position = new Vector3(pck.Position[0], pck.Position[1], pck.Position[2]),
          Yaw = pck.Yaw,
          Tick = pck.Tick,
          Stop = true
        });
        break;
    }
  }
}
