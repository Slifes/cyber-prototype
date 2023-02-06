using Godot;

using System;
using System.Collections.Generic;

enum EnemyState
{
  Living,
  Battle,
  Died
}

partial class BaseEnemy : BaseNPC
{
  [Signal]
  public delegate void BehaviorChangeStateEventHandler();

  [Signal]
  public delegate void BehaviorUpdateStateEventHandler();

  private Area3D HitBox;

  private Behavior behavior;

  public Skill CurrentSkill;

  public AnimationPlayer Animation { get; set; }

  List<int> nearest;

  public override void _Ready()
  {
    onActorReady();

    HitBox = GetNode<Area3D>("HitBox");
    HitBox.BodyEntered += HitBodyEntered;

    nearest = new();

    Animation = GetNode<AnimationPlayer>("AnimationPlayer");
  }

  private void HitBodyEntered(Node3D body)
  {
    if (body.Name != Name)
    {
      GD.Print("Hitted Player");
      IActor actor = (IActor)body;

      actor.TakeDamage(5);
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    behavior.Update(delta);
  }

  public void ExecuteSkill(int skillId)
  {
    Animation.Play(String.Format("Skills/{0}", skillId));

    ServerBridge.Instance.SendSkillExecutedTo(nearest, this, skillId);
  }

  public override Variant GetData()
  {
    var data = new Godot.Collections.Array<Variant>()
        {
            ID,
            currentHP,
            currentSP,
            maxHP,
            maxSP,
            (int)state,
            behavior.GetData(),
        };

    return data;
  }

  public override void TakeDamage(int damage)
  {
    base.TakeDamage(damage);

    if (currentHP <= 0)
    {
      this.ChangeState(AIState.Died);
    }

    ServerBridge.Instance.SendActorTookDamage(this.nearest, this, damage);
  }
}
