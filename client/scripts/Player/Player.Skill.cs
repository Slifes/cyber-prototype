using Godot;

public partial class Player
{
    public void RunSkill(Variant index)
    {
        int i = index.AsInt32();

        var skill = (PackedScene)ResourceLoader.Load("res://skills/normal_attack.tscn");

        var node  = skill.Instantiate();

        GD.Print("Skill: ", node);

        if (node != null && !skillNode.HasNode(node.Name.ToString()))
        {
            skillNode.AddChild(node);

            node.Call("play_animation");
        }
    }

    public void InputSkill()
    {
        if (Input.IsActionJustReleased("attack"))
        {
            GD.Print("Inputskill");
            GetNode<WorldState>("../../WorldState").SendRequestSkill(0);
        }
    }
}
