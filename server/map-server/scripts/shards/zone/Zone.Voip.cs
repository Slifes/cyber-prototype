﻿using Godot;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using MessagePack;
using System.Collections.Generic;

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
    }
    catch (Exception e)
    {
      GD.Print("Failed to connect to proxy client: " + e.Message);
      GD.PrintErr(e);
    }
  }

  public async void SendShardAuthentication()
  {
    await SendPacket(new ShardAuthentication()
    {
      ShardId = 1,
      ShardKey = Name
    });
  }

  public async void SendPlayerConnected(int peerId)
  {
    await SendPacket(new ShardPlayerConnect()
    {
      PlayerId = peerId
    });
  }

  public async void SendPlayerDisconnected(int peerId)
  {
    await SendPacket(new ShardPlayerDisconnected()
    {
      PlayerId = peerId
    });
  }

  async Task SendPacket<T>(T packet)
  {
    await stream.WriteAsync(MessagePackSerializer.Serialize(packet));
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
