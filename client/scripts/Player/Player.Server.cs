using Godot;

partial class Player : CharacterBody3D
{
    [Export]
    float PredictionDecayFactor = 0.9f;

    [Export]
    float PredictionUpdateRate = 0.1f;

    double LastUpdateTime = WorldState.Now();

    Vector3 PredictedVelocity = Vector3.Zero;

    Vector3 PredictedPosition = Vector3.Zero;

    Vector3 LastPositionUpdated = Vector3.Zero;


    bool isMoving = false;

    void UpdatePrediction(float delta)
    {
        //var elapsedTime = (float)(WorldState.Now() - LastUpdateTime);

        PredictedPosition = GlobalPosition + PredictedVelocity * (delta * 1000);

        // GD.Print("PredictedPosition: ", PredictedPosition);
        // GD.Print("Elapsed: ", ((float)elapsedTime) / 1000);
    }

    void UpdatePosition(Vector3 newPosition)
    {
        var now = WorldState.Now();

        PredictedVelocity = (newPosition - GlobalPosition) / (float)(now - LastUpdateTime);

        GD.Print("PredictedVelocity", PredictedVelocity);

        GlobalPosition = newPosition;

        LastPositionUpdated = newPosition;

        PredictedPosition = newPosition;

        LastUpdateTime = WorldState.Now();
    }

    void InterpolatePosition()
    {
        var elapsedTime = (float)(WorldState.Now() - LastUpdateTime);

        GlobalPosition = ((LastPositionUpdated + (PredictedVelocity * elapsedTime)) * PredictionDecayFactor) + (LastPositionUpdated * (1 - PredictionDecayFactor));
    }

    void ServerMovement(Variant position, Variant yaw)
    {
		GD.Print("Position: ", position);
        GD.Print("yaw: ", yaw);

        isMoving = true;
        UpdatePosition((Vector3)position);
        SetActorRotation(new Vector3(0, (float)yaw, 0));
    }

    void ServerMovementStopped(Variant position, Variant yaw)
    {
        GD.Print("Stopped");
        isMoving = false;
        UpdatePosition((Vector3)position);
        SetActorRotation(new Vector3(0, (float)yaw, 0));
    }

    void _ServerUpdatePosition(float delta)
    {
        if (isMoving)
        {
            //UpdatePrediction(delta);
            InterpolatePosition();
        }
    }
}