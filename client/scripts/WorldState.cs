using System.Reactive;
using Godot;
using Godot.Collections;

struct Action
{
	public string ActorId;
	public string Function;
	public Variant[] Data;
	public double Timestamp;
}

partial class WorldState: Node3D
{
	struct State
	{
		public double timestamp;
		public Dictionary<Variant, Variant> players;
	}

	SpawnerCustom spawner;

	Node3D Network;

	System.Collections.Generic.List<State> states;

	System.Collections.Generic.List<Action> actions;

	int InterpolationOffset = 100;

	public static double Now()
	{
		return Time.GetUnixTimeFromSystem() * 1000.0;
	}

	public override void _Ready()
	{
		Network = GetParent<Node3D>();

		spawner = GetNode<SpawnerCustom>("../Players");

		states = new();

		actions = new();
	}

	public override void _PhysicsProcess(double delta)
	{
		double clientClock = Network.Get("client_clock").AsDouble();
		double renderTime = clientClock - InterpolationOffset;

		/*if (states.Count > 1)
		{
			while(states.Count > 2 && renderTime > states[1].timestamp)
			{
				states.RemoveAt(0);
			}

			var currentState = states[0];
			var nextState = states[1];

			var timestamp = currentState.timestamp;
			var timestamp1 = nextState.timestamp;

			var interpolationFactor = (float)((renderTime - timestamp) / (timestamp1 - timestamp));

			UpdatePlayerState(
				currentState.players,
				nextState.players,
				interpolationFactor
			);
		}*/

		RunActions(renderTime);
	}

	private void RunActions(double timestamp)
	{
		for (var i = actions.Count -1; i > -1; i--)
		{
			var action = actions[i];

			if (timestamp > action.Timestamp)
			{
				if ((timestamp - action.Timestamp) < 100)
				{
					var node = spawner.GetNode(action.ActorId);

					if (node != null)
					{
						node.Call(action.Function, action.Data);
					}
				}

				actions.RemoveAt(i);
			}
		}
	}

	private void UpdatePlayerState(Variant currentState, Variant nextState, float interpolationFactor)
	{
		var player0 = currentState.AsGodotDictionary();
		var player1 = nextState.AsGodotDictionary();

		foreach (var player in player0.Keys)
		{
			string name = player.ToString();

			if (name == GetParent().Multiplayer.GetUniqueId().ToString())
			{
				continue;
			}

			if (spawner.HasNode(name) && player0.ContainsKey(player) && player1.ContainsKey(player))
			{
				var playerObj = player0[player].AsVector3Array();
				var playerObj1 = player1[player].AsVector3Array();

				var position = playerObj[0];
				var position1 = playerObj1[0];

				var rotation = playerObj[1];
				var rotation1 = playerObj1[1];

				var node = spawner.GetNode<Player>(name);

				node.GlobalPosition = position.Lerp(position1, interpolationFactor);
				node.SetActorRotation(rotation.Lerp(rotation1, interpolationFactor));
			}
		}
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveWorldState(double timestamp, Variant players)
	{
		/*states.Add(new State
		{
			timestamp = timestamp,
			players = players.AsGodotDictionary<Variant, Variant>()
		});*/
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorEnteredZone(Variant id, Variant position)
	{
		GD.Print("Player entered zone", id);
		spawner.Spawn(id, (Vector3)position);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void ActorExitedZone(Variant id)
	{
		GD.Print("Player exited zone: ", id);
		spawner.Unspawn(id);
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void SpawnActorPlayable(Variant id)
	{
		spawner.SpawnPlayableActor(id, Vector3.Up);
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

	public void SendRequestSkill(Variant id)
	{
		RpcId(1, "RequestSkill", id);
	}
}
