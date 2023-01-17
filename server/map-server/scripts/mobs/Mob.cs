using System;
using System.Collections.Generic;
using Godot;

enum MobState
{
    Idle,
    Walking,
    Attacking
}

partial class Mob: Actor
{
    [Export]
    float Speed = 1.0f;

    [Export]
    FastNoiseLite noise;

    NavigationAgent3D agent;

    Random random;

    Vector3 targetLocation;

    MobState state;

    Area3D area;

    double LastIdleTimeChecked;

    List<int> nearest;

    float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Ready()
    {
        base._Ready();

        SetMultiplayerAuthority(1);

        area = GetNode<Area3D>("AABB");
        area.BodyEntered += Area_BodyEntered;
        area.BodyExited += Area_BodyExited;

        agent = GetNode<NavigationAgent3D>("NavigationAgent3D");

        random = new Random();

        LastIdleTimeChecked = Time.GetUnixTimeFromSystem();

        nearest = new();
    }

    private void Area_BodyExited(Node3D body)
    {
        Actor actor = (Actor)body;

        if (actor.IsPlayer() && nearest.Contains(actor.ActorID))
        {
            nearest.Remove(actor.ActorID);
        }
    }

    private void Area_BodyEntered(Node3D body)
    {
        Actor actor = (Actor)body;

        if (actor.IsPlayer())
        {
            nearest.Add(actor.ActorID);
        }
    }

    private void SendToAllActors(string func, params Variant[] args)
    {
        foreach(var actorID in nearest)
        {
            RpcId(actorID, func, args);
        }
    }

    private void HandleIdle(double delta)
    {
        var now = Time.GetUnixTimeFromSystem();

        if ((now - LastIdleTimeChecked) > 2)
        {
            var index = random.Next(10);

            if (index % 2 == 0)
            {
                state = MobState.Walking;
            }

            LastIdleTimeChecked = now;
        }
    }

    bool FixY(float delta)
    {
        if (!IsOnFloor())
        {
            Vector3 velocity = Velocity;

            velocity.y -= gravity * delta;

            Velocity = velocity;

            MoveAndSlide();

            return false;
        }

        return true;
    }

    private void HandleWalking(double delta)
    {
        if (targetLocation == Vector3.Zero)
        {
            var random = new RandomNumberGenerator();

            var mobX = random.Randf() * 4;
            var mobY = random.Randf() * 4;

            GD.Print("New position: ", mobX, mobY);

            targetLocation = new Vector3(mobX, GlobalPosition.y, mobY);

            agent.TargetLocation = targetLocation;
        }

        if (agent.IsTargetReachable())
        {
            if (agent.IsTargetReached())
            {
                state = MobState.Idle;
                targetLocation = Vector3.Zero;
            }
            else
            {
                var next = agent.GetNextLocation();
                var velocity = GlobalPosition.DirectionTo(next).Normalized() * Speed * (float)delta;

                Velocity = velocity;

                SendToAllActors("MoveToTarget", GlobalPosition, velocity);

                MoveAndSlide();
            }
        }
        else
        {
            targetLocation = Vector3.Zero;
            state = MobState.Idle;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (FixY((float)delta))
        {
            if (state == MobState.Idle)
            {
                HandleIdle(delta);
            }
            else
            {
                HandleWalking(delta);
            }
        }
    }

    [RPC(TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
    public void MoveToTarget(Vector3 position, Vector3 velocity) { }

    public override Variant GetData()
    {
        var baseData = (Godot.Collections.Array)base.GetData();

        baseData.Add((int)state);
        baseData.Add(Velocity);

        return baseData;
    }
}