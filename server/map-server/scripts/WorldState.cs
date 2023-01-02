using Godot;
using Godot.Collections;

partial class WorldState : Node3D
{
	Node3D players;

	Array<Variant> worldStates;

	public override void _Ready()
	{
		players = (Node3D)GetNode("../Players");
	}

	public override void _PhysicsProcess(double delta)
	{
		RpcId(0, "ReceiveWorldState", Time.GetUnixTimeFromSystem() * 1000.0, GetPlayerState());
	}

	private Dictionary<Variant, Variant> GetPlayerState()
	{
		Dictionary<Variant, Variant> playerState = new();

		var playerCount = players.GetChildCount();
		GD.Print("playercount: ", playerCount);

		for (var i = 0; i < playerCount; i++)
		{
			var playerNode = (CharacterBody3D)players.GetChild(i);

			Godot.Collections.Dictionary<Variant, Variant> player = new()
			{
				{"position", Variant.CreateFrom(playerNode.GlobalPosition)}
			};

			playerState.Add(Variant.CreateFrom(playerNode.Name), Variant.CreateFrom(player));
		}

		GD.Print(playerState);

		return playerState;
	}

	[RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveWorldState(double timestamp, Variant players) { }
}
