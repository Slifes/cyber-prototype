using MessagePack;

namespace Packets.Client
{
  [MessagePackObject]
  public partial struct PlayerStartMovement : IClientCommand
  {
    [Key(0)] public float[] Position;
    [Key(1)] public float Yaw;
  }

  [MessagePackObject]
  public partial struct PlayerStopMovement : IClientCommand
  {
    [Key(0)] public float[] Position;
    [Key(1)] public float Yaw;
  }

  [MessagePackObject]
  public partial struct PlayerRequestSkill : IClientCommand
  {
    [Key(0)] public int skillId;
  }
}
