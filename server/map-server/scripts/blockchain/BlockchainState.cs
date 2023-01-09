using GameServer.scripts.blockchain;
using Godot;

public partial class BlockchainState : Node3D
{
	Chain chain = new();

	bool finished = true;

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (finished)
		{
			GD.Print("Started to look");
			finished = false;
			chain.Pool().ContinueWith(t =>
			{
				GD.Print("Finished to look");
				finished = true;
			});
		}
	}
}
