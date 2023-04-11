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

    _instance = this;

    sceneMultiplayer = new SceneMultiplayer();

    sceneMultiplayer.ServerRelay = false;
    sceneMultiplayer.PeerPacket += OnPacketReceived;

    GetTree().SetMultiplayer(sceneMultiplayer);

    multiplayerPeer = new ENetMultiplayerPeer();

    sceneMultiplayer.PeerConnected += _PeerConnected;
    sceneMultiplayer.PeerDisconnected += _PeerDisconnected;

    var error = multiplayerPeer.CreateServer(9060, 1000);

    GD.Print("Server network error: ", error);

    sceneMultiplayer.MultiplayerPeer = multiplayerPeer;
  }

  void _PeerConnected(long id)
  {
    GD.Print("Connected: ", id);
  }

  void _PeerDisconnected(long id)
  {
    GD.Print("Disconnected: ", id);

    PlayerSpawner.Instance.Despawn(id);
  }

  void OnPacketReceived(long id, byte[] data)
  {
    packetManager.OnPacketHandler(id, data);
  }

  byte[] Serialize(Packets.Server.IServerCommand cmd)
  {
    return MessagePackSerializer.Serialize<Packets.Server.IServerCommand>(cmd);
  }

  public void SendPacket(long id, Packets.Server.IServerCommand cmd)
  {
    var pck = Serialize(cmd);

    sceneMultiplayer.SendBytes(pck, (int)id);
  }

  public void SendPacketToMany(List<int> peers, Packets.Server.IServerCommand cmd)
  {
    var pck = Serialize(cmd);

    peers.ForEach((peerId) => sceneMultiplayer.SendBytes(pck, peerId));
  }

  public void SendPacketToMany(int peer, List<int> peers, Packets.Server.IServerCommand cmd)
  {
    var pck = Serialize(cmd);

    sceneMultiplayer.SendBytes(pck, peer);

    peers.ForEach((peerId) => sceneMultiplayer.SendBytes(pck, peerId));
  }

  public void Disconnect(int peerId)
  {
    sceneMultiplayer.DisconnectPeer(peerId);
  }
}
