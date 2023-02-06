using Godot;

partial class Player
{
  public void SendMoving()
  {
    RpcId(1, "SendMovement", new Vector2(GlobalPosition.X, GlobalPosition.Z), GetBodyRotation().Y);
  }

  public void SendMoveStopped()
  {
    RpcId(1, "SendMovementStopped", new Vector2(GlobalPosition.X, GlobalPosition.Z), GetBodyRotation().Y);
  }

  public void SendRequestSkill(Variant id, Variant data)
  {
    RpcId(1, "RequestSkill", id, data);
  }

  #region rpc
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void SendMovement(Variant position, Variant yaw) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
  public void SendMovementStopped(Variant position, Variant yaw) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant id, Variant data) { }
  #endregion
}
