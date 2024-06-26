﻿using Godot;

class BaseAttack : IBehavior
{
  Behavior behavior;

  private float time;

  private float targetTime = 1;

  private Vector3 LastOrigin;

  private Area3D AttackArea;

  private EnemyRandomSkill randomSkill;

  public BaseAttack(Behavior actor)
  {
    this.behavior = actor;

    this.randomSkill = new EnemyRandomSkill(actor.Actor);

    AttackArea = actor.Actor.GetNode<Area3D>("AttackArea");
  }

  public void Finish()
  {
    AttackArea.BodyExited -= BodyExited;
  }

  public void Handler(double delta)
  {
    if (time >= targetTime)
    {
      randomSkill.Execute();

      time = 0;
    }
    else
    {
      time += (float)delta;
    }

    var offset = (behavior.Target.GlobalPosition - LastOrigin) * 2f * (float)delta;
    LastOrigin = LastOrigin + offset;

    behavior.Actor.LookAt(LastOrigin, Vector3.Up);

    if (offset > Vector3.Zero)
    {
      // actor.UpdateState();
    }
  }

  public void Start()
  {
    AttackArea.BodyExited += BodyExited;

    LastOrigin = behavior.Target.GlobalPosition;
  }

  private void BodyExited(Node3D body)
  {
    if (body.Name == behavior.Target.Name)
    {
      if (body.IsInsideTree())
      {
        behavior.ChangeState(AIState.Steering);
      }
      else
      {
        behavior.Target = null;
        behavior.ChangeState(AIState.Walking);
      }
    }
  }

  public Variant GetData()
  {
    return Vector3.Zero;
  }
}
