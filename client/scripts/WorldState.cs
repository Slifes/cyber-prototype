using System.Collections.Generic;
using Godot;
using Godot.Collections;

partial class WorldState: Node3D
{
	struct State
	{
		public double timestamp;
		public Godot.Collections.Dictionary<Variant, Variant> players;
	}

	SpawnerCustom spawner;

	List<State> states;

	int InterpolationOffset = 100;

	public override void _Ready()
	{
		spawner = GetNode<SpawnerCustom>("../Players");

		states = new List<State>();
	}

	public override void _PhysicsProcess(double delta)
	{
		var renderTime = (Time.GetUnixTimeFromSystem() * 1000.0) - InterpolationOffset;

		if (states.Count > 1)
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

			if (spawner.HasNode(name))
			{
				var playerObj = player0[player].AsGodotDictionary<Variant, Variant>();
				var playerObj1 = player1[player].AsGodotDictionary<Variant, Variant>();

				var position = (Vector3)playerObj["position"];
				var position1 = (Vector3)playerObj1["position"];

				var node = (CharacterBody3D)spawner.GetNode(name);

				node.GlobalPosition = position.Lerp(position1, interpolationFactor);
			}
		}
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveWorldState(double timestamp, Variant players)
	{
		GD.Print(timestamp);

		states.Add(new State
		{
			timestamp = timestamp,
			players = players.AsGodotDictionary<Variant, Variant>()
		});
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
}
