using System.Linq;
using Godot;

struct Action
{
  public string ActorId;
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

	public static double Now()
	{
		return Time.GetUnixTimeFromSystem() * 1000.0;
	}

	public override void _Ready()
	{
		Network = GetParent<Node3D>();

		spawner = GetNode<Spawner>("../Spawner");

		actions = new();
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
			.GroupBy(x => x.ActorId);

		foreach (var actor in acts)
		{
			var player = spawner.GetPlayer(actor.Key.ToString());
		
			if (player != null)
			{
				foreach (var act in actor)
				{
					player.Call(act.Function, act.Data);

					actions.Remove(act);
				}
			} else
			{
				foreach (var act in actor)
				{
					actions.Remove(act);
				}
			}
		}
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
	public void SendRequestSkill(Variant id)
	{
		RpcId(1, "RequestSkill", id);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void RequestSkill(Variant id) { }

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void SkillApproved(Variant playerId, Variant skillId, Variant timestamp)
	{
		GD.Print("Received skill approved");

		actions.Add(new Action
		{
			ActorId = playerId.ToString(),

			Function = "RunSkill",

			Data = new Variant[1] { skillId },

			Timestamp = (double)timestamp
		});
	}
	#endregion

	#region movement
	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveMovement(Variant actorId, Variant position, Variant yaw, Variant timestamp) {

		actions.Add(new Action
		{
			ActorId = actorId.ToString(),
			Function = "ServerMovement",
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
}
