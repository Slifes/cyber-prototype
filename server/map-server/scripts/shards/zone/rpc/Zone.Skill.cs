using Godot;
using Packets.Server;

partial class Zone
{
  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void UseItem(int actorId, int itemId)
  {
    var item = ItemManager.Instance.Get(itemId);

    GD.Print("Item: ", item);

    if (item != null && item.Type == ItemType.Active)
    {
      var usable = (Usable)item;

      var actor = (PlayerZone)spawner.Get(actorId);

      GD.Print("Send Usable skill");

      actor.EmitSignal(ZoneActor.SignalName.ExecuteSkill, usable.skillId, new Variant());
    }
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(int actorId, int skillId, Variant data)
  {
    var actor = spawner.Get((int)actorId);

    if (actor != null)
    {
      actor.EmitSignal(ZoneActor.SignalName.ExecuteSkill, skillId, data);
    }
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ExecuteSkill(int actorId, int actorType, int skillId, Variant data)
  {
    SendPacketToAllNearestAndMe(actorId, new SMActorExecuteSkill
    {
      ActorId = actorId,
      ActorType = actorType,
      SkillId = skillId,
    });
  }
}
