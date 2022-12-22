using GameServer.scripts;
using Godot;

using Thread = Godot.Thread;

public partial class BlockchainState : Node3D
{
	public override void _Ready()
	{
		var thread = new Thread();

		Error err = thread.Start(new Callable(this, "StartChain"));

		GD.Print(err);
	}

	void StartChain()
	{
		Chain chain = new();

		while (true)
		{
			chain.pool();

			OS.DelayMsec(5000);
		}
	}
}
