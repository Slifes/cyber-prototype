using Godot;

using System;
using System.Collections.Generic;

enum AIState
{
  Idle,
  Walking,
  Steering,
  Attacking,
  Died
}

partial class BaseEnemyActor : CharacterActor
{
  AIState state = AIState.Idle;

  IBehavior behavior;

  Dictionary<AIState, IBehavior> behaviors;

  public AnimationPlayer Animation { get; set; }

  public override void _Ready()
  {
    onActorReady();

    Animation = GetNode<AnimationPlayer>("AnimationPlayer");

    behaviors = new()
		{
			{AIState.Steering, new BasedContextSteering(this)},
			{AIState.Walking, new BaseMovement(this)},
			{AIState.Attacking, new BaseAttack(this)},
			{AIState.Died, new BaseDied(this)}
		};

    ChangeState(AIState.Walking);
  }

  public override void _PhysicsProcess(double delta)
  {
    if (behavior != null)
    {
      behavior.Handler(delta);
    }
  }

  public void ChangeState(AIState state, Variant data = new Variant())
  {
    GD.Print("New state: ", state);

    if (behavior != null)
    {
      behavior.Finish();
    }

    behavior = behaviors[state];
    behavior.SetData(data);
    behavior.Start();

    this.state = state;
  }

  public void ReceiveChangeState(Variant state, Variant position, Variant yaw, Variant data)
  {
    AIState _state = (AIState)(int)state;

    GlobalPosition = (Vector3)position;
    RotationDegrees = new Vector3(0, (float)yaw, 0);

    ChangeState(_state, data);
  }

  public void ReceiveUpdateState(Variant state, Variant position, Variant yaw, Variant data)
  {
    GlobalPosition = (Vector3)position;
    RotationDegrees = new Vector3(0, (float)yaw, 0);

    if (behavior != null)
    {
      behavior.SetData(data);
    }
  }

  public override void SetServerData(Variant data)
  {
    var dataArray = data.AsGodotArray<Variant>();

    var actorReference = (int)dataArray[0];
    currentHP = (int)dataArray[1];
    currentSP = (int)dataArray[2];
    maxHP = (int)dataArray[3];
    maxSP = (int)dataArray[4];

    var state = (AIState)(int)dataArray[5];

    ChangeState(state, dataArray[6]);
  }

  public override void ExecuteSkill(Variant skillId)
  {
    int i = skillId.AsInt32();

    Animation.Play(String.Format("Skills/{0}", i));
  }

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);

    GetNode<Damage>("/root/World/Damage").Spawn(this, damage);
  }
}
