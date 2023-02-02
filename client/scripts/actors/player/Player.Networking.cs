using Godot;

partial class Player
{
  void ServerMovement(Variant position, Variant yaw)
  {
    if (State != PlayerState.Walking)
      ChangeState(PlayerState.Walking);

    ((PredictMovement)components[0]).UpdatePosition((Vector3)position);
    SetBodyRotation(new Vector3(0, (float)yaw, 0));
  }

  void ServerMovementStopped(Variant position, Variant yaw)
  {
    ChangeState(PlayerState.Idle);

    ((PredictMovement)components[0]).UpdatePosition((Vector3)position);
    SetBodyRotation(new Vector3(0, (float)yaw, 0));
  }

  public void SendMoving()
  {
    RpcId(1, "SendMovement", new Vector2(GlobalPosition.X, GlobalPosition.Z), GetBodyRotation().Y);
  }

  public void SendMoveStopped()
  {
    RpcId(1, "SendMovementStopped", new Vector2(GlobalPosition.X, GlobalPosition.Z), GetBodyRotation().Y);
  }

  #region rpc
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovement(Variant position, Variant yaw) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovementStopped(Variant position, Variant yaw) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant id, Variant data) { }
  #endregion
}
