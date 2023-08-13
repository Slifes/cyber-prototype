using Godot;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using MessagePack;

[MessagePackObject]
public partial struct ShardAuthentication
{
  [Key(0)] public uint PacketId => 1;
  [Key(1)] public uint ShardId;
  [Key(2)] public string ShardKey;
}

[MessagePackObject]
public partial struct ShardPlayerConnect
{
  [Key(0)] public uint PacketId => 2;
  [Key(1)] public int PlayerId;
}

[MessagePackObject]
public partial struct ShardPlayerDisconnected
{
  [Key(0)] public uint PacketId => 3;
  [Key(1)] public int PlayerId;
}

[MessagePackObject]
public partial struct ShardPlayerAddCloser
{
  [Key(0)] public uint PacketId => 4;
  [Key(1)] public int PlayerId;
  [Key(2)] public int CloserId;
}

[MessagePackObject]
public partial struct ShardPlayerRemoveCloser
{
  [Key(0)] public uint PacketId => 5;
  [Key(1)] public int PlayerId;
  [Key(2)] public int CloserId;
}

[MessagePackObject]
public partial struct SMAuth
{
  [Key(0)] public bool Status;
}

partial class ProxyClient : GodotObject
{
  TcpClient client;

  NetworkStream stream;

  GodotThread threadHandler;

  public string Name { get; set; }

  public ProxyClient(string Name)
  {
    this.Name = Name;
  }

  public void Connect(string ip, int port)
  {
    GD.Print("Connecting to proxy client...");

    client = new TcpClient();

    try
    {
      client.Connect(ip, port);

      stream = client.GetStream();

      SendShardAuthentication();

      threadHandler = new GodotThread();
      threadHandler.Start(new Callable(this, nameof(OnThreadHandler)));
    }
    catch (Exception e)
    {
      GD.Print("Failed to connect to proxy client: " + e.Message);
      GD.PrintErr(e);
    }
  }

  public void SendShardAuthentication()
  {
    SendPacket(new ShardAuthentication()
    {
      ShardId = 1,
      ShardKey = Name
    });
  }

  public void SendPlayerConnected(int peerId)
  {
    SendPacket(new ShardPlayerConnect()
    {
      PlayerId = peerId
    });
  }

  public void SendPlayerDisconnected(int peerId)
  {
    SendPacket(new ShardPlayerDisconnected()
    {
      PlayerId = peerId
    });
  }

  public void SendPlayerAddCloser(int peerId, int closerId)
  {
    SendPacket(new ShardPlayerAddCloser()
    {
      PlayerId = peerId,
      CloserId = closerId
    });
  }

  public void SendPlayerRemoveCloser(int peerId, int closerId)
  {
    SendPacket(new ShardPlayerRemoveCloser()
    {
      PlayerId = peerId,
      CloserId = closerId
    });
  }

  void SendPacket<T>(T packet)
  {
    var data = MessagePackSerializer.Serialize(packet);
    stream.Write(data, 0, data.Length);
  }

  void OnThreadHandler()
  {
    GD.Print("Thread running");
    while (true)
    {
      try
      {
        var buffer = new byte[1024];

        var read = stream.Read(buffer, 0, buffer.Length);

        if (read > 0)
        {
          GD.Print("Packet arrived: ", read);
        }
      }
      catch (Exception e)
      {
        GD.Print("Failed to read packet: " + e.Message);
        GD.PrintErr(e);
      }
    }
  }
}

partial class Zone
{
  ProxyClient proxyClient;

  void InitializeProxyClient()
  {
    proxyClient = new(Name);
    proxyClient.Connect("127.0.0.1", 8080);
  }
}
