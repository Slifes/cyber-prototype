using Godot;
using System.Collections.Generic;

partial class Zone
{
  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorEnteredZone(Variant actorId, Variant id, Variant type)
  {
	if (!neraests.ContainsKey((int)id))
	{ 
	  neraests.Add((int)actorId, new List<int>() { (int)id });
	}
	else 
	{
	  neraests[(int)actorId].Add((int)id);
	}
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorExitedZone(Variant actorId, Variant id, Variant type)
  {
	if (neraests.ContainsKey((int)actorId))
	{
	  neraests[(int)actorId].Remove((int)id);
	}
  }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorMoved(Variant actorId, Variant position, Variant yaw)
  {
    Node3D actor = spawner.Get((int)actorId);
    
    actor.GlobalPosition = (Vector3)position;
    actor.Rotation = new Vector3(0, (float)yaw, 0);
  }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcChangeState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void NpcUpdateState(Variant id, Variant state, Variant position, Variant yaw, Variant data, Variant timestamp) { }

  [Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void ActorTookDamage(Variant actorId, Variant actorType, Variant damage, Variant hp, Variant maxHP) { }

  [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
  public void RequestSkill(Variant actorId, Variant skillId, Variant data)
  {
    var actor = spawner.Get((int)actorId);

    if (actor != null)
    {
      actor.EmitSignal("ExecuteSkill", skillId, data);
    }
  }
}