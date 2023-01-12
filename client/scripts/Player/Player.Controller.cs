using Godot;
using System;

partial class Player
{
	[Export]
	float MouseWheelVelocity = .5f;

	[Export]
	float MouseWheelUpLimit = 1.0f;

	[Export]
	float MouseWheelDownLimit = 10.0f;

	float time = 0.0f;
		
	bool moveStoppedSended = false;

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton emb = (InputEventMouseButton)@event;
			if (emb.IsPressed())
			{
				if (emb.ButtonIndex == MouseButton.WheelUp && camera3d.Size > MouseWheelUpLimit)
				{
					camera3d.Size -= MouseWheelVelocity;
					GD.Print("Camera Size: ", camera3d.Size);
				}
				if (emb.ButtonIndex == MouseButton.WheelDown && camera3d.Size < MouseWheelDownLimit)
				{
					camera3d.Size += MouseWheelVelocity;
				}
			}
		}
	}

	void _RotateCamera(double delta)
	{
		bool isPressed = Input.IsMouseButtonPressed(MouseButton.Right);

		if (isPressed)
		{
			if (mouseCameraPressed)
			{
				Vector2 currentMousePosition = GetViewport().GetMousePosition();

				float x = mouseMoveCameraInitial.x - currentMousePosition.x;

				float velocity = 0.2f * x * (float)delta;

				camera.RotateY(velocity);

				mouseMoveCameraInitial = currentMousePosition;
			}
			else
			{
				mouseCameraPressed = true;
				mouseMoveCameraInitial = GetViewport().GetMousePosition();
			}
		}
		else
		{
			mouseCameraPressed = false;
		}
	}

	void _MoveCharacter(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.y -= gravity * (float)delta;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");

		if (inputDir != Vector2.Zero)
		{
			body.Rotation = camera.Rotation;

			Vector3 direction = (body.Transform.basis * new Vector3(inputDir.x, 0, inputDir.y)).Normalized();

			velocity.x = direction.x * Speed;
			velocity.z = direction.z * Speed;

			moveStoppedSended = false;
		}
		else
		{
			velocity.x = Mathf.MoveToward(Velocity.x, 0, Speed);
			velocity.z = Mathf.MoveToward(Velocity.z, 0, Speed);
		}

		Velocity = velocity;

		MoveAndSlide();

		time += (float)delta;

		if (Velocity == Vector3.Zero)
		{
			if (!moveStoppedSended)
			{
				SendMoveStopped();
				moveStoppedSended = true;
			}

		} else if (time > 1.0f/20.0f)
		{
			SendMoving();

			time = 0.0f;
		}
	}

	void SendMoving()
	{
		RpcId(1, "SendMovement", new Vector2(GlobalPosition.x, GlobalPosition.z), GetActorRotation().y);
	}

	void SendMoveStopped()
	{
		RpcId(1, "SendMovementStopped", new Vector2(GlobalPosition.x, GlobalPosition.z), GetActorRotation().y);
	}

	void _AuthorityController(double delta)
	{
		_RotateCamera(delta);
		_MoveCharacter(delta);
		InputSkill();
	}
}
