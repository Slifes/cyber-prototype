﻿using MessagePack;

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

  [MessagePackObject]
  public partial struct PlayerUseItem : IClientCommand
  {
    [Key(0)] public int itemId;
  }

  [MessagePackObject]
  public partial struct PlayerPickUpItem : IClientCommand
  {
    [Key(0)] public int dropID;
  }

  [MessagePackObject]
  public partial struct CMAudioVoiceData : IClientCommand
  {
    [Key(0)] public byte[] data;
  }

  [MessagePackObject]
  public partial struct CMStartAudioVoice : IClientCommand
  {
    [Key(0)] public int channel;
  }

  [MessagePackObject]
  public partial struct CMUpdateAudioVoice : IClientCommand
  {
    [Key(0)] public int channel;
    [Key(1)] public byte[] data;
  }

  [MessagePackObject]
  public partial struct CMEndAudioVoice : IClientCommand
  {
    [Key(0)] public int channel;
  }
}
