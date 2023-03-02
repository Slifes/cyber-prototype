using Godot;
using Packets.Client;

partial class Player
{
  public void SendMoving()
  {
    NetworkManager.Instance.SendPacket(new PlayerStartMovement
    {
      Position = new float[3] {Position.X, Position.Y, Position.Z},
      Yaw = GetBodyRotation().Y
    });
  }

  public void SendMoveStopped()
  {
    NetworkManager.Instance.SendPacket(new PlayerStopMovement
    {
      Position = new float[3] {Position.X, Position.Y, Position.Z},
      Yaw = GetBodyRotation().Y
    });
  }

  public void SendRequestSkill(int id, Variant data)
  {
    NetworkManager.Instance.SendPacket(new PlayerRequestSkill
    {
      skillId = id
    });
  }
}
