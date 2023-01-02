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

	Node3D players;

	List<State> states;

	int InterpolationOffset = 20;

	public override void _Ready()
	{
		players = (Node3D)GetNode("../Players");

		states = new List<State>();
	}

	public override void _PhysicsProcess(double delta)
	{
		GD.Print("Running");
		var renderTime = (Time.GetUnixTimeFromSystem() * 1000.0) - InterpolationOffset;

		if (states.Count > 1)
		{
			while(states.Count > 2 && renderTime > states[1].timestamp)
			{
				states.RemoveAt(0);
			}

			GD.Print("Calculation");

			var currentState = states[0];
			var nextState = states[1];

			GD.Print("currentState: ", currentState);

			var timestamp = currentState.timestamp;
			var timestamp1 = nextState.timestamp;

			GD.Print("Timestamp: ", timestamp);

			var interpolationFactor = (float)((renderTime - timestamp) / (timestamp1 - timestamp));

			GD.Print("Interpolation factor: ", interpolationFactor);

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

			GD.Print("Name: ", name);
			GD.Print("Sender: ", GetParent().Multiplayer.GetUniqueId());

			if (name == GetParent().Multiplayer.GetUniqueId().ToString())
			{
				continue;
			}

			if (players.HasNode(name))
			{
				var playerObj = player0[player].AsGodotDictionary<Variant, Variant>();
				var playerObj1 = player1[player].AsGodotDictionary<Variant, Variant>();

				var position = (Vector3)playerObj["position"];
				var position1 = (Vector3)playerObj1["position"];

				GD.Print("Position0: ", position);
				GD.Print("Position1: ", position1);
			
				var node = (CharacterBody3D)players.GetNode(name);

				node.GlobalPosition = position.Lerp(position1, interpolationFactor);

				GD.Print("New Position: ", node.GlobalPosition);
			}
		}
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveWorldState(double timestamp, Variant players)
	{
		GD.Print("Received state");
		GD.Print(timestamp);

		states.Add(new State
		{
			timestamp = timestamp,
			players = players.AsGodotDictionary<Variant, Variant>()
		});
	}
}
