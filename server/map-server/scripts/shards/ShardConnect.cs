using Godot;

partial class ShardConnect : Node
{
  [Export]
  public bool IsServer;

  [Export]
  public int Port;

	public int PID;

  protected ENetMultiplayerPeer multiplayerPeer;

  SceneMultiplayer MultiplayerCustom;

  public override void _Ready()
  {
	if (IsServer)
	{
	  CreateServer(Port);
	}
	else
	{
	  Connect(Port);
	}
  }

  public void CreateServer(int port)
  {
	GD.Print("Create server: ", port);
	MultiplayerCustom = new SceneMultiplayer();

	multiplayerPeer = new ENetMultiplayerPeer();

	MultiplayerCustom.ServerRelay = false;

	MultiplayerCustom.PeerConnected += _PeerConnected;
	MultiplayerCustom.PeerDisconnected += _PeerDisconnected;

	var error = multiplayerPeer.CreateServer(port);

	GD.Print("Server shard error: ", error);

	MultiplayerCustom.MultiplayerPeer = multiplayerPeer;

	GetTree().SetMultiplayer(MultiplayerCustom);
  }

  public void Connect(int port)
  {
	GD.Print("Connect to shard: ", port);
	MultiplayerCustom = new SceneMultiplayer();

	multiplayerPeer = new ENetMultiplayerPeer();

	MultiplayerCustom.PeerConnected += _PeerConnected;
	MultiplayerCustom.PeerDisconnected += _PeerDisconnected;

	var error = multiplayerPeer.CreateClient("127.0.0.1", port);

	GD.Print("Connect shard error: ", error);

	MultiplayerCustom.MultiplayerPeer = multiplayerPeer;

	GetTree().SetMultiplayer(MultiplayerCustom, GetParent().GetPath());
  }

  void _PeerConnected(long id)
  {
	GD.Print("MainServer Connected");

  }

  void _PeerDisconnected(long id) 
  {

  }
}
