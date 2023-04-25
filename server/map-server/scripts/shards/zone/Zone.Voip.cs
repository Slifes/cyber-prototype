using Godot;
using System.Net.Sockets;
using System.Threading.Tasks;
using MessagePack;
using System.Collections.Generic;

[MessagePackObject]
public partial struct ShardAuthentication : IVoipCommand
{
  [Key(0)] public uint PacketId => 1;
  [Key(1)] public uint ShardId;
  [Key(2)] public string ShardKey;
}

[MessagePackObject]
public partial struct ShardPlayerConnect : IVoipCommand
{
  [Key(0)] public uint PacketId => 2;
  [Key(1)] public uint PlayerId;
}

[Union(1, typeof(ShardAuthentication))]
[Union(2, typeof(ShardPlayerConnect))]
public interface IVoipCommand { }

class GodotVoip
{
  TcpClient client;

  Mutex mutex = new Mutex();

  GodotThread threadHandler;

  List<IVoipCommand> queue = new();

  public void Connect(string ip, int port)
  {
    client = new TcpClient();

    client.Connect(ip, port);

    client.GetStream().Write(MessagePackSerializer.Serialize(new ShardAuthentication()
    {
      ShardId = 20,
      ShardKey = "test"
    }));
  }

  public void SendPackets()
  {
    mutex.Lock();

    queue.ForEach((IVoipCommand data) =>
    {
      client.GetStream().Write(MessagePackSerializer.Serialize(data));
    });

    mutex.Free();
  }
}

partial class Zone
{
  GodotVoip voip;

  void ConnectToVoip()
  {
    voip = new();
    voip.Connect("127.0.0.1", 8080);

    // var threadVoip = new GodotThread();

    // threadVoip.Start(new Callable(this, nameof(ConnectToVoipThread)));
  }

  void ConnectToVoipThread()
  {
    var voip = new TcpClient();

    voip.Connect("127.0.0.1", 8080);
  }
}
