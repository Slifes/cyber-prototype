using System.Linq;
using Godot;

struct Action
{
  public string ActorId;
  public ActorType ActorType;
  public string Function;
  public Variant[] Data;
  public double Timestamp;
}

partial class ServerBridge: Node3D
{
  Spawner spawner;

  Node3D Network;

  System.Collections.Generic.List<Action> actions;

  int InterpolationOffset = 100;

  private static ServerBridge _instance;

  public static ServerBridge Instance
  {
    get { return _instance; }
  }

  public static double Now()
  {
    return Time.GetUnixTimeFromSystem() * 1000.0;
  }

  public override void _Ready()
  {
    Network = GetParent<Node3D>();

    spawner = GetNode<Spawner>("../Spawner");

    actions = new();

    _instance = this;

    SkillManager.CreateInstance();
    SkillManager.Instance.Load();
  }

  public override void _PhysicsProcess(double delta)
  {
    double clientClock = Network.Get("client_clock").AsDouble();
    double renderTime = clientClock - InterpolationOffset;

    RunActions(renderTime);
  }

  private void RunActions(double timestamp)
  {
    var acts = actions
      .FindAll(x => x.Timestamp < timestamp)
      .OrderBy(x => x.Timestamp)
      .GroupBy(x => new { x.ActorId, x.ActorType });

    ulong start = 0;

    foreach (var actorData in acts)
    {
      start = Time.GetTicksUsec();

      var actor = (Node3D)spawner.GetActor(actorData.Key.ActorId.ToString(), actorData.Key.ActorType);
    
      if (actor != null)
      {
        foreach (var act in actorData)
        {
          actor.Call(act.Function, act.Data);
        }
      }

      GD.Print("Time action: ", Time.GetTicksUsec() - start);
    }

    actions.RemoveAll(x => x.Timestamp < timestamp);
  }

  #region spawn
  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(Variant id, Variant type, Variant position, Variant data)
  {
    GD.Print("Actor entered zone", id);

    spawner.Spawn(id, type, position, data);
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(Variant id, Variant type)
  {
    ActorType actorType = (ActorType)(int)type;

    GD.Print("Actor exited zone: ", id);

    spawner.Unspawn(id, type);
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorPlayable(Variant id, Variant position, Variant data)
  {
    spawner.Spawn(id, (Variant)(int)ActorType.Player, position, data);
  }
  #endregion

  #region skill
  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SkillExecuted(Variant actorId, Variant actorType, Variant skillId, Variant timestamp)
  {
    GD.Print("Received skill approved");

    actions.Add(new Action
    {
      ActorId = actorId.ToString(),
      ActorType = (ActorType)(int)actorType,
      Function = "ExecuteSkill",
      Data = new Variant[1] { skillId },
      Timestamp = (double)timestamp
    });
  }
  #endregion

  #region movement
  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp)
  {
    actions.Add(new Action
    {
      ActorId = actorId.ToString(),
      Function = "ServerMovement",
      ActorType = ActorType.Player,
      Data = new Variant[2]
      {
        position,
        yaw
      },
      Timestamp = (double)timestamp
    });
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void ReceiveMovementStopped(Variant actorId, Variant position, Variant yaw, Variant timestamp)
  {
    actions.Add(new Action
    {
      ActorId = actorId.ToString(),
      ActorType = ActorType.Player,
      Function = "ServerMovementStopped",
      Data = new Variant[2]
      {
        position,
        yaw
      },
      Timestamp = (double)timestamp
    });
  }
  #endregion

  #region npc
  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp)
  {
    actions.Add(new Action
    {
      ActorId = id.ToString(),
      ActorType = ActorType.Npc,
      Function = "ReceiveChangeState",
      Data = new Variant[4]
      {
        state,
        position,
        yaw,
        data
      },
      Timestamp = (double)timestamp
    });
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcUpdateState(Variant id, Variant state,Variant position, Variant yaw, Variant data, Variant timestamp)
  {
    actions.Add(new Action
    {
      ActorId = id.ToString(),
      ActorType = ActorType.Npc,
      Function = "ReceiveUpdateState",
      Data = new Variant[4]
      {
        state,
        position,
        yaw,
        data
      },
      Timestamp = (double)timestamp
    });
  }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcAction(Variant id, Variant action, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorTookDamage(Variant actorId, Variant actorType, Variant damage, Variant hp, Variant maxHP)
  {
    IActor actor = spawner.GetActor(actorId.ToString(), (ActorType)(int)actorType);

    if(actor != null)
    {
      actor.TakeDamage((int)damage);
    }
    
  }
  #endregion
}
