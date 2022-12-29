using Godot;
using System;

public partial class PlayerNetwork : Node
{
	[Export]
	public Vector3 position;
	
	[Export]
	public Vector3 Direction;

	[Export]
	public int State;
}
