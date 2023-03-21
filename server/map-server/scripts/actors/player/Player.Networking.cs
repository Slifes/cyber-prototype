using Godot;
using Packets.Client;

partial class Player
{
  public void StartMovement(PlayerStartMovement command)
  {
    this.state = ActorState.Walking;

    var position = new Vector3(command.Position[0], command.Position[1], command.Position[2]);

    SendPacketToZone("ActorStartMove", GetActorId(), position, command.Yaw);
  }

  public void StopMovement(PlayerStopMovement command)
  {
    this.state = ActorState.Idle;

    var position = new Vector3(command.Position[0], command.Position[1], command.Position[2]);

    SendPacketToZone("ActorStopMove", GetActorId(), position, command.Yaw);
  }

  public void RequestSkill(PlayerRequestSkill command)
  {
    GD.Print("Received Request skill: ", GetActorId());

    SendPacketToZone("RequestSkill", GetActorId(), command.skillId, new Variant());
  }

  public void UseItem(PlayerUseItem command)
  {
    if (inventory.Contains(command.itemId))
    {
      SendPacketToZone("UseItem", GetActorId(), command.itemId);

      inventory.Remove(command.itemId, 1);
    }
  }
}
