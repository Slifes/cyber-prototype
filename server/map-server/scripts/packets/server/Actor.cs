using MessagePack;

namespace Packets.Server
{
  [MessagePackObject]
  public partial struct SMActorEnteredZone : IServerCommand
  {
    [Key(0)] public int ActorId;
    [Key(1)] public int ActorType;
    [Key(2)] public float[] Position;
    [Key(3)] public float Yaw;
    [Key(4)] public byte[] Data;
  }

  [MessagePackObject]
  public partial struct SMActorExitedZone : IServerCommand
  {
    [Key(0)] public int ActorId;
    [Key(1)] public int ActorType;
  }

  [MessagePackObject]
  public partial struct SMExecuteSkill : IServerCommand
  {
    [Key(0)] public int ActorId;
    [Key(1)] public int ActorType;
    [Key(2)] public int SkillId;
  }
}
