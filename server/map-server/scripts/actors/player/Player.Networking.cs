using Godot;

partial class Player
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SendMovement(Variant position, Variant yaw)
  {
    Move((Vector2)position, (float)yaw, (int)ActorState.Walking);

    ServerBridge.Instance.SendServerMovement(this, GlobalPosition, (float)yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SendMovementStopped(Variant position, Variant yaw)
  {
    Move((Vector2)position, (float)yaw, (int)ActorState.Walking);

    ServerBridge.Instance.SendServerMovementStopped(this, GlobalPosition, (float)yaw);
  }


  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant id, Variant data)
  {
    GD.Print("Received Request skill: ", GetActorId());

    ServerBridge.Instance.SendSkillExecutedTo(GetNearestPlayers(), this, (int)id);

    GetComponent<SkillHandler>().ExecuteSkill(id);
  }
}
