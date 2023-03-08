using Godot;
using MessagePack;
using Packets.Client;

partial class NetworkManager : Node3D
{
  static NetworkManager _instance;

  public static NetworkManager Instance { get { return _instance; } }

  AuthClient auth;

  Label LatencyText;

  Label PacketLossText;

  SceneMultiplayer sceneMultiplayer;

  ENetMultiplayerPeer clientPeer;

  ENetPacketPeer serverPeer;

  PacketManager packetManager;

  public ulong FirstPickTick { get; set; }

  public ulong ServerTickSent { get; set; }

  public ulong ServerTick { get; set; }

  public override void _Ready()
  {
	_instance = this;

	auth = GetNode<AuthClient>("../AuthClient");
	LatencyText = GetNode<Label>("UI/Network/Ping");
	PacketLossText = GetNode<Label>("UI/Network/Loss");

	packetManager = new PacketManager();

	SkillManager.CreateInstance();
	SkillManager.Instance.Load();
  }

  public override void _EnterTree()
  {
	StartNetwork();
  }

  void StartNetwork()
  {
	sceneMultiplayer = new SceneMultiplayer();

	clientPeer = new ENetMultiplayerPeer();

	sceneMultiplayer.ConnectedToServer += ConnectedToServer;
	sceneMultiplayer.ServerDisconnected += Disconnected;
	sceneMultiplayer.PeerPacket += OnPeerPacket;

	clientPeer.CreateClient("127.0.0.1", 4242);

	sceneMultiplayer.MultiplayerPeer = clientPeer;

	GetTree().SetMultiplayer(sceneMultiplayer);
  }

  void ConnectedToServer()
  {
	serverPeer = clientPeer.GetPeer(1);

	var authToken = "pGmmzfP3tYZybrYbFLr6SVJKVA4";

	GD.Print("Sending session token: ", authToken);

	SyncServerTime();

	SendPacket(new EnterSessionMap { AuthToken = authToken });
  }

  void Disconnected()
  {
	GetTree().Quit();
  }

  void OnPeerPacket(long id, byte[] data)
  {
	packetManager.OnPacketHandler(id, data);
  }

  void SyncServerTime()
  {
	var now = Time.GetUnixTimeFromSystem() * 1000.0f;
	var pck = new FetchServerTime { ClientTime = now };

	SendPacket(pck);
  }

  public void SendPacket(IClientCommand command)
  {
	var pck = MessagePackSerializer.Serialize<IClientCommand>(command);

	sceneMultiplayer.SendBytes(pck, 1, mode: MultiplayerPeer.TransferModeEnum.Reliable);
  }

  public override void _PhysicsProcess(double delta)
  {
	if (serverPeer != null)
	{
	  LatencyText.Text = string.Format("Ping: {0}ms", serverPeer.GetStatistic(ENetPacketPeer.PeerStatistic.RoundTripTime));
	  PacketLossText.Text = string.Format("Loss: {0}", serverPeer.GetStatistic(ENetPacketPeer.PeerStatistic.PacketLoss));
	}

	ServerTick = ServerTickSent + (Time.GetTicksMsec() - FirstPickTick);
  }
}
