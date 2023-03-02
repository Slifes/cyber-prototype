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

partial class ActionManager : Node3D
{
  //   Spawner spawner;

  //   NetworkManager Network;

  //   System.Collections.Generic.List<Action> actions;

  //   int InterpolationOffset = 100;

  //   public override void _Ready()
  //   {
  // 	Network = GetParent<NetworkManager>();

  // 	spawner = GetNode<Spawner>("../Spawner");

  // 	actions = new();
  // 	SkillManager.CreateInstance();
  // 	SkillManager.Instance.Load();
  //   }

  //   public override void _PhysicsProcess(double delta)
  //   {
  // 	double renderTime = (int)NetworkManager.Instance.ServerTick - InterpolationOffset;

  // 	RunActions(renderTime);
  //   }

  //   private void RunActions(double timestamp)
  //   {
  // 	var acts = actions
  // 	  .FindAll(x => x.Timestamp < timestamp)
  // 	  .OrderBy(x => x.Timestamp)
  // 	  .GroupBy(x => new { x.ActorId, x.ActorType });

  // 	ulong start = 0;

  // 	foreach (var actorData in acts)
  // 	{
  // 	  start = Time.GetTicksUsec();

  // 	  var actor = (Node3D)spawner.GetActor(actorData.Key.ActorId.ToString(), actorData.Key.ActorType);

  // 	  if (actor != null)
  // 	  {
  // 		foreach (var act in actorData)
  // 		{
  // 		  actor.EmitSignal(act.Function, act.Data);
  // 		}
  // 	  }

  // 	  GD.Print("Time action: ", Time.GetTicksUsec() - start);
  // 	}

  // 	actions.RemoveAll(x => x.Timestamp < timestamp);
  //   }

  //   [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  //   public void SkillExecuted(Variant actorId, Variant actorType, Variant skillId, Variant timestamp)
  //   {
  // 	GD.Print("Received skill approved");
  // 	GD.Print("H: ", CharacterActor.SignalName.ExecuteSkill);

  // 	actions.Add(new Action
  // 	{
  // 	  ActorId = actorId.ToString(),
  // 	  ActorType = (ActorType)(int)actorType,
  // 	  Function = CharacterActor.SignalName.ExecuteSkill,
  // 	  Data = new Variant[1] { skillId },
  // 	  Timestamp = (double)timestamp
  // 	});
  //   }

  //   [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  //   public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp)
  //   {
  // 	actions.Add(new Action
  // 	{
  // 	  ActorId = actorId.ToString(),
  // 	  Function = Player.SignalName.SvStartMovement,
  // 	  ActorType = ActorType.Player,
  // 	  Data = new Variant[2]
  // 	  {
  // 		position,
  // 		yaw
  // 	  },
  // 	  Timestamp = (double)timestamp
  // 	});
  //   }

  //   [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  //   public void ReceiveMovementStopped(Variant actorId, Variant position, Variant yaw, Variant timestamp)
  //   {
  // 	actions.Add(new Action
  // 	{
  // 	  ActorId = actorId.ToString(),
  // 	  ActorType = ActorType.Player,
  // 	  Function = Player.SignalName.SvStopMovement,
  // 	  Data = new Variant[2]
  // 	  {
  // 		position,
  // 		yaw
  // 	  },
  // 	  Timestamp = (double)timestamp
  // 	});
  //   }

  //   [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  //   public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp)
  //   {
  // 	actions.Add(new Action
  // 	{
  // 	  ActorId = id.ToString(),
  // 	  ActorType = ActorType.Npc,
  // 	  Function = BaseEnemy.SignalName.SvBehaviorSetState,
  // 	  Data = new Variant[4]
  // 	  {
  // 		state,
  // 		position,
  // 		yaw,
  // 		data
  // 	  },
  // 	  Timestamp = (double)timestamp
  // 	});
  //   }

  //   [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  //   public void NpcUpdateState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp)
  //   {
  // 	actions.Add(new Action
  // 	{
  // 	  ActorId = id.ToString(),
  // 	  ActorType = ActorType.Npc,
  // 	  Function = BaseEnemy.SignalName.SvBehaviorUpdateState,
  // 	  Data = new Variant[4]
  // 	  {
  // 		state,
  // 		position,
  // 		yaw,
  // 		data
  // 	  },
  // 	  Timestamp = (double)timestamp
  // 	});
  //   }

  //   [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  //   public void ActorTookDamage(Variant actorId, Variant actorType, Variant damage, Variant hp, Variant maxHP)
  //   {
  // 	GD.Print("Damage");
  // 	actions.Add(new Action
  // 	{
  // 	  ActorId = actorId.ToString(),
  // 	  ActorType = (ActorType)(int)actorType,
  // 	  Function = CharacterActor.SignalName.TakeDamage,
  // 	  Data = new Variant[3]
  // 	  {
  // 		damage,
  // 		hp,
  // 		maxHP
  // 	  },
  // 	  Timestamp = (double)Now()
  // 	});

  //   }
}
