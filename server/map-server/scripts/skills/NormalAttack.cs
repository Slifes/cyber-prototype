using Godot;

partial class NormalAttack: Node3D, ISkillScene
{
    AnimationPlayer animPlayer;

    Area3D areaCollision;

    public override void _Ready()
    {
        animPlayer = GetNode<AnimationPlayer>("animation");

        areaCollision = GetNode<Area3D>("Pivot/Body/Area3D");

        areaCollision.BodyEntered += AreaCollision_BodyEntered;
    }

    private void AreaCollision_BodyEntered(Node3D body)
    {
        ((Player)body).ApplyDamage(10);
    }

    public void RunSkillAnimation()
    {
        animPlayer.Play();
    }
}