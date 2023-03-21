﻿using Packets.Server;

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
    var pck = (SMActorExecuteSkill)command;

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

  void OnActorEffect(IServerCommand command)
  {
    var pck = (SMActorEffect)command;

    var actor = Spawner.Instance.GetActor<CharacterActor>(pck.ActorId);

    actor.EmitSignal(CharacterActor.SignalName.Effect, pck.EffectType, pck.Value);
  }
}
