using System;
using System.Linq;
using System.Xml.Linq;
using Godot;
using Godot.Collections;

partial class Player: CharacterBody3D
{
	Area3D aabb;

	WorldState worldState;

	[Export]
	Dictionary<Variant, CharacterBody3D> nearest;

	public Dictionary<Variant, CharacterBody3D> GetNearestPlayers()
	{
		return nearest;
	}

	public override void _Ready()
	{
		SetMultiplayerAuthority(Int32.Parse(Name));

		worldState = GetNode<WorldState>("../../WorldState");

		aabb = GetNode<Area3D>("AABB");
		aabb.BodyEntered += OnBodyEntered;
		aabb.BodyExited += OnBodyExited;

		nearest = new();
	}

	private void OnBodyEntered(Node3D body)
	{
		if (!nearest.ContainsKey(body.Name))
		{
			nearest.Add(body.Name, (CharacterBody3D)body);

			worldState.SendActorEnteredZone(int.Parse(Name), body.Name, (Variant)GlobalPosition);
		}
	}

	private void OnBodyExited(Node3D body)
	{
		if (nearest.ContainsKey(body.Name))
		{
			nearest.Remove(body.Name);

			worldState.SendActorExitedZone(int.Parse(Name), body.Name);
		}
	}

	[RPC(MultiplayerAPI.RPCMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
	public void ReceiveState(Variant position)
	{
		if (Name == Multiplayer.GetRemoteSenderId().ToString())
		{
			GlobalPosition = (Vector3)position;
		}
	}
}
