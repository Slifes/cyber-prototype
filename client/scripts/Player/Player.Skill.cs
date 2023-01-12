using Godot;

partial class Player
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

            camera3d.Call("add_trauma", 0.15f);

            node.Call("play_animation");
        }
    }

    public void InputSkill()
    {
        if (Input.IsActionJustPressed("attack"))
        {
            GD.Print("Inputskill");
            GetNode<ServerBridge>("../../Server").SendRequestSkill(0);
        }
    }
}
