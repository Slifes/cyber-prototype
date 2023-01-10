using Godot;
using Godot.Collections;

partial class MobActor: Actor
{
    enum MobState
    {
        Idle,
        Walking,
    }

    enum Stance
    {
        Attacking,
    }

    MobState state;

    Vector3 velocity;

    public override void _PhysicsProcess(double delta)
    {
        if (state == MobState.Walking)
        {
            Velocity = velocity / 20;

            MoveAndSlide();
        }
    }

    [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
    public void MoveToTarget(Vector3 position, Vector3 velocity)
    {
        this.velocity = velocity;
        GlobalPosition = position;
        state = MobState.Walking;
    }

    public void SetData(Variant data)
    {
        var ar = (Array)data;

        state = (MobState)(int)ar[5];
        velocity = (Vector3)ar[6];
    }
}