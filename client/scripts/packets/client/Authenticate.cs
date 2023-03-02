using MessagePack;

namespace Packets.Client
{
  [MessagePackObject]
  public partial struct EnterSessionMap : IClientCommand
  {
    [Key(0)] public string AuthToken;
  }

  [MessagePackObject]
  public partial struct FetchServerTime : IClientCommand
  {
    [Key(0)] public double ClientTime;
  }
}
