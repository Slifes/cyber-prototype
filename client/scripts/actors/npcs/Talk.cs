using Godot;

enum TalkType
{
  Talk,
  Mechanic,
  Shop
}

partial class Talk : BaseNPC
{
  [Export]
  public Resource Dialogue;

  [Export]
  public TalkType Type;

  protected override IComponent[] CreateComponents()
  {
    return new IComponent[3]
    {
      new ActorHover(this),
      new Dialogue(this),
      new Clickable(this)
    };
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void StartTalk(Variant actorId) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  void StopTalk(Variant actorId) { }
}
