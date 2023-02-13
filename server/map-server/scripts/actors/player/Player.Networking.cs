using Godot;

partial class Player
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SendMovement(Variant position, Variant yaw)
  {
    this.state = ActorState.Walking;

    //zone.SendMovementStarted(this, position, yaw);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SendMovementStopped(Variant position, Variant yaw)
  {
    this.state = ActorState.Idle;

    //zone.SendMovementStopped(this, position, yaw);
  }


  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant id, Variant data)
  {
    // GD.Print("Received Request skill: ", GetActorId());

    // ServerBridge.Instance.SendSkillExecutedTo(GetNearestPlayers(), this, (int)id);

    // skillHandler.ExecuteSkill(id);
  }
}
