using System;
using Godot;
using Godot.Collections;

enum ActorType
{
    Player,
    Npc
}

partial class Actor: CharacterBody3D
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

    public int CurrentHP { get { return currentHP; } }

    public int CurrentSP { get { return currentSP; } }

    public int MaxHP { get { return maxHP; } }

    public int MaxSP { get { return maxSP; } }

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

    public virtual Variant GetData()
    {
        return new Array<Variant>()
        {
            actorReference,
            currentHP,
            currentSP,
            maxHP,
            maxSP,
        };
    }

    public virtual void SetData(Variant data)
    {
        var arrayData = (Array<Variant>)data;

        currentHP = (int)arrayData[1];
        currentSP = (int)arrayData[2];
        maxHP = (int)arrayData[3];
        maxSP = (int)arrayData[4];
    }
}
