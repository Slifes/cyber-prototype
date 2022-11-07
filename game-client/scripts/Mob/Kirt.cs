using Godot;
using System;

public partial class Kirt : CharacterBody3D
{
	public const float Speed = 30.0f;

	RayCast3D rayCast;

	Node3D target;

	AnimationPlayer anim;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		rayCast = (RayCast3D)GetNode("MeshInstance3D/RayCast3D");
		anim = (AnimationPlayer)GetNode("AnimationPlayer");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (rayCast.IsColliding() && target == null)
		{
			var obj = rayCast.GetCollider();

			Node3D mayTarget = (Node3D)obj;

			if(mayTarget.IsInGroup("Player"))
			{
				rayCast.Enabled = false;
				anim.Play("Attack");
				target = mayTarget;
			}
		}

		if (target != null)
		{
			var direction = (target.GlobalPosition - GlobalPosition).Normalized();
			var distance = GlobalPosition.DistanceTo(target.GlobalPosition);


			if (distance > .4f)
			{
				velocity = new Vector3(direction.x, 0, direction.z) * (Speed * (float)delta);
			} else
			{
				velocity = Vector3.Zero;
			}
		}

		Velocity = velocity;

		MoveAndSlide();
	}
}
