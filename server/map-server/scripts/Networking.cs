using Godot;
using System.Collections.Generic;
using MessagePack;

public partial class Networking : Node3D
{
  static Networking _instance;

  public static Networking Instance { get { return _instance; } }

  SceneMultiplayer sceneMultiplayer;

  ENetMultiplayerPeer multiplayerPeer;

  PacketManager packetManager;

  public override void _Ready()
  {
    packetManager = new PacketManager();

    SkillManager.CreateInstance("skills");
    SkillManager.Instance.Load();
  }

  public override void _EnterTree()
  {
    _instance = this;

    sceneMultiplayer = new SceneMultiplayer();

    sceneMultiplayer.ServerRelay = false;
    sceneMultiplayer.PeerPacket += OnPacketReceived;

    GetTree().SetMultiplayer(sceneMultiplayer);

    multiplayerPeer = new ENetMultiplayerPeer();

    Multiplayer.PeerConnected += _PeerConnected;
    Multiplayer.PeerDisconnected += _PeerDisconnected;

    multiplayerPeer.CreateServer(4242, 1000);

    Multiplayer.MultiplayerPeer = multiplayerPeer;
  }

  void _PeerConnected(long id)
  {
    GD.Print("Connected: ", id);
  }

  void _PeerDisconnected(long id)
  {
    GD.Print("Disconnected: ", id);
  }

  void OnPacketReceived(long id, byte[] data)
  {
    packetManager.OnPacketHandler(id, data);
  }

  public void SendPacket(long id, Packets.Server.IServerCommand cmd)
  {
    var pck = MessagePackSerializer.Serialize<Packets.Server.IServerCommand>(cmd);

    sceneMultiplayer.SendBytes(pck, (int)id);
  }

  public void SendPacketToMany(List<int> peers, Packets.Server.IServerCommand cmd)
  {
    var pck = MessagePackSerializer.Serialize<Packets.Server.IServerCommand>(cmd);

    foreach (int peerId in peers)
    {
      sceneMultiplayer.SendBytes(pck, peerId);
    }
  }

  public void Disconnect(int peerId)
  {
    sceneMultiplayer.DisconnectPeer(peerId);
  }
}
