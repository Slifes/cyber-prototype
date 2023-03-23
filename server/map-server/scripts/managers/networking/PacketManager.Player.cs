using Packets.Client;

partial class PacketManager
{
  void OnPlayerStartMovement(long peerId, Player actor, IClientCommand command)
  {
    actor.StartMovement((PlayerStartMovement)command);
  }

  void OnPlayerStopMovement(long peerId, Player actor, IClientCommand command)
  {
    actor.StopMovement((PlayerStopMovement)command);
  }

  void OnPlayerRequestSkill(long peerId, Player actor, IClientCommand command)
  {
    actor.RequestSkill((PlayerRequestSkill)command);
  }

  void OnPlayerUseItem(long peerId, Player actor, IClientCommand command)
  {
    actor.UseItem((PlayerUseItem)command);
  }

  void OnPlayerPickUpItem(long peerId, Player actor, IClientCommand command)
  {
    actor.PickUp((PlayerPickUpItem)command);
  }
}
