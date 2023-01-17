using Godot;

partial class Player
{
  PackedScene normalAttack = ResourceLoader.Load<PackedScene>("res://skills/normal_attack.tscn");

  public void RunSkill(Variant index)
  {
    int i = index.AsInt32();

    var node  = normalAttack.Instantiate();

    GD.Print("Skill: ", node);

    if (node != null && !skillNode.HasNode(node.Name.ToString()))
    {
      skillNode.AddChild(node);

      node.Call("play_animation");
    }
  }

  public void InputSkill()
  {
    if (Input.IsActionJustPressed("attack"))
    {
      GD.Print("Inputskill");
      GetNode<ServerBridge>("/root/World/Server").SendRequestSkill(0);
    }
  }
}
