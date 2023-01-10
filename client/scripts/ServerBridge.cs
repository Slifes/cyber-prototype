using System;
using System.Linq;
using Godot;
using Godot.Collections;

struct Action
{
	public string ActorId;
	public string Function;
	public Variant[] Data;
	public double Timestamp;
}

partial class ServerBridge: Node3D
{
	CharacterSpawner characterSpawner;

	MobSpawner npcSpawner;

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

		characterSpawner = GetNode<CharacterSpawner>("../Players");

		npcSpawner = GetNode<MobSpawner>("../Mobs");

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
			if (characterSpawner.HasNode(actor.Key))
			{
				var node = characterSpawner.GetNode(actor.Key);

				foreach (var act in actor)
				{
					if (node != null)
					{
						node.Call(act.Function, act.Data);
					}

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

		if (((ActorType)(int)type) == ActorType.Player)
		{
			characterSpawner.Spawn(id, (Vector3)position, data);
		} else
		{
			npcSpawner.Spawn(id, (Vector3)position, data);
		}
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorExitedZone(Variant id, Variant type)
	{
		ActorType actorType = (ActorType)(int)type;

		GD.Print("Actor exited zone: ", id);

		if (actorType == ActorType.Player)
		{
			characterSpawner.Unspawn(id);
		}

		else if (actorType == ActorType.Npc)
		{
			npcSpawner.Unspawn(id);
		}
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorPlayable(Variant id, Variant position, Variant data)
	{
		characterSpawner.SpawnPlayableActor(id, (Vector3)Position, data);
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
