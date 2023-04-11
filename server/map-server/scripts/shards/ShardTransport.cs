using Godot;

partial class ShardTransport : Node
{
  [Export]
  public bool IsServer;

  [Export]
  public int Port;

  [Export]
  public int PID;

  [Export]
  public bool Debug;

  [Export]
  public bool AutoLoad = true;

  ENetMultiplayerPeer multiplayerPeer;

  SceneMultiplayer MultiplayerCustom;

  public override void _EnterTree()
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
    if (AutoLoad)
    {
      PID = OS.CreateProcess(OS.GetExecutablePath(), new string[] { (Debug ? "" : "--headless"), "shard", GetParent().Name }, false);
    }

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
    GD.Print("Server: ", Name);
  }

  void _PeerDisconnected(long id)
  {
    GetTree().Quit();
  }
}
