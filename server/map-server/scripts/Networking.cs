using Godot;

public partial class Networking : Node3D
{
  ENetMultiplayerPeer multiplayerPeer;

  CharacterSpawner spawner;

  public override void _EnterTree()
  {
    var sceneMultiplayer = new SceneMultiplayer();

    sceneMultiplayer.ServerRelay = false;

    GetTree().SetMultiplayer(sceneMultiplayer);

    multiplayerPeer = new ENetMultiplayerPeer();

    Multiplayer.PeerConnected += _PeerConnected;
    Multiplayer.PeerDisconnected += _PeerDisconnected;

    multiplayerPeer.CreateServer(4242, 1000);

    Multiplayer.MultiplayerPeer = multiplayerPeer;
  }

  public override void _Ready()
  {
    spawner = GetNode<CharacterSpawner>("Spawner/players");
  }

  void _PeerConnected(long id)
  {
    GD.Print("Connected: ", id);
  }

  void _PeerDisconnected(long id)
  {
    GD.Print("Disconnected: ", id);

    spawner.CallDeferred("Unspawn", id);
  }

  void Spawn(int remoteId)
  {
    var actor = spawner.Spawn(remoteId, Vector3.Up, new Godot.Collections.Array<Variant>()
    {
      0,
      100,
      100,
      100,
      100
    });

    ServerBridge.Instance.SendPlayableActor(remoteId, actor);
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void FetchServerTime(Variant clientTime)
  {
    double now = Time.GetUnixTimeFromSystem() * 1000.0;

    RpcId(Multiplayer.GetRemoteSenderId(), "ReturnServerTime", now, clientTime);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ReturnServerTime(Variant serverTime, Variant clientTime) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void onSessionMap(Variant auth_token)
  {
    GD.Print("Requesting: Entering map.");

    int remoteId = Multiplayer.GetRemoteSenderId();

    string token = auth_token.ToString();

    /*using var db = new ServerContext();
	var sessions = db.Sessions.Where(x => x.AuthToken == token);

	if (!sessions.Any())
	{
		Multiplayer.MultiplayerPeer.DisconnectPeer(remoteId);
	}

	foreach (var session in sessions)
	{
		GD.Print("session map found");
		GD.Print(session.Id);
		GD.Print(session.AuthToken);
		GD.Print(session.CharacterId);

		Spawn(remoteId);
	}*/
    Spawn(remoteId);

    GD.Print(auth_token);
  }
}
