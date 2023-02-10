using Godot;

partial class ActorNetwork : CharacterBody3D
{
  [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
  public void Disconnected() { QueueFree(); }
}
