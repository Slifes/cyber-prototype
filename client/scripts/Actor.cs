using System;
using Godot;

enum ActorType
{
    Player,
    Npc
}

partial class Actor : CharacterBody3D
{
    public int ActorID { get { return _actorId; } }

    protected int _actorId;

    public ActorType Type { get { return _type; } }

    protected ActorType _type;

    public int actorReference;

    protected int currentHP;

    protected int currentSP;

    protected int maxHP;

    protected int maxSP;

    public override void _Ready()
    {
        _actorId = Int32.Parse(Name);

        maxHP = 100;
        maxSP = 100;

        currentHP = maxHP;
        currentSP = maxSP;
    }

    public bool IsPlayer()
    {
        return Type == ActorType.Player;
    }
}
