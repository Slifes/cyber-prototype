using Packets.Server;

partial class PacketManager
{
  void OnActorEnteredZone(IServerCommand command)
  {
	Spawner.Instance.Spawn((SMActorEnteredZone)command);
  }

  void OnActorExitedZone(IServerCommand command)
  {
	Spawner.Instance.Unspawn((SMActorExitedZone)command);
  }

  void OnActorExecuteSkill(IServerCommand command)
  {
	var pck = (SMExecuteSkill)command;

	var actor = Spawner.Instance.GetActor<CharacterActor>(pck.ActorId);

	actor.EmitSignal(CharacterActor.SignalName.ExecuteSkill, pck.SkillId);
  }

  void ActorPushCommand(int actorId, IServerCommand command)
  {
	var actor = Spawner.Instance.GetActor<CharacterActor>(actorId);

	actor.PushCommand(command);
  }

  void OnActorStartMove(IServerCommand command)
  {
	var pck = (SMActorStartMove)command;

	ActorPushCommand(pck.ActorId, command);
  }

  void OnActorStopMove(IServerCommand command)
  {
	var pck = (SMActorStopMove)command;

	ActorPushCommand(pck.ActorId, command);
  }

  void OnActorTakeDamage(IServerCommand command)
  {
	var pck = (SMActorDamage)command;

	var actor = Spawner.Instance.GetActor<CharacterActor>(pck.ActorId);

	actor.EmitSignal(CharacterActor.SignalName.TakeDamage, pck.Damage, actor.GetCurrentHP(), actor.GetMaxHP());
  }
}
