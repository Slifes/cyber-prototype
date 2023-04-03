using Godot;
using Packets.Server;

partial class Zone
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorDrop(int actorId, int actorType, int money, Variant data)
  {
    var items = data.AsGodotArray();
    var converted = new DroppedItem[items.Count];

    for (var i = 0; i < items.Count; i++)
    {
      var instance = items[i].AsGodotDictionary();
      converted[i].dropId = instance["dropId"].AsInt32();
      converted[i].itemId = instance["itemId"].AsInt32();
    }

    SendPacketToAllNearest(actorId, new SMActorDroppedItems
    {
      ActorId = actorId,
      ActorType = actorType,
      Items = converted
    });
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void PickUp(int actorId, int dropId)
  {
    var actor = (ZoneActor)spawner.Get(actorId);

    dropItems.PickUp(actor, dropId);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void DropItemRemove(int dropId)
  {
    SendPacketToAllInZone(new SMDroppedItemRemove
    {
      dropId = dropId
    });
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void DropCollected(int actorId, int dropId, int itemId)
  {
    PlayerSpawner.Instance
      .GetNode<Player>(actorId.ToString())
      .Inv
      .Add(itemId, 1);

    SendPacketToAllInZone(new SMDroppedItemRemove
    {
      dropId = dropId
    });
  }
}
