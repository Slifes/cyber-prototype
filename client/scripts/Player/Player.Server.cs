using Godot;

partial class Player
{
    float PredictionDecayFactor = 0.9f;

    double LastUpdateTime = ServerBridge.Now();

    Vector3 PredictedVelocity = Vector3.Zero;

    Vector3 LastPositionUpdated = Vector3.Zero;

    bool isMoving = false;

    void UpdatePosition(Vector3 newPosition)
    {
        var now = ServerBridge.Now();

        PredictedVelocity = (newPosition - GlobalPosition) / (float)(now - LastUpdateTime);

        GD.Print("PredictedVelocity", PredictedVelocity);

        GlobalPosition = newPosition;

        LastPositionUpdated = newPosition;

        LastUpdateTime = ServerBridge.Now();
    }

    void InterpolatePosition()
    {
        var elapsedTime = (float)(ServerBridge.Now() - LastUpdateTime);

        GlobalPosition = ((LastPositionUpdated + (PredictedVelocity * elapsedTime)) * PredictionDecayFactor) + (LastPositionUpdated * (1 - PredictionDecayFactor));
    }

    void ServerMovement(Variant position, Variant yaw)
    {
        isMoving = true;
        UpdatePosition((Vector3)position);
        SetActorRotation(new Vector3(0, (float)yaw, 0));
    }

    void ServerMovementStopped(Variant position, Variant yaw)
    {
        isMoving = false;
        UpdatePosition((Vector3)position);
        SetActorRotation(new Vector3(0, (float)yaw, 0));
    }

    void _ServerUpdatePosition(float delta)
    {
        if (isMoving)
        {
            InterpolatePosition();
        }
    }
}