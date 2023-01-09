using Godot;

enum SkillType
{
    Active,
    Passive
}

interface ISkillScene
{
    void RunSkillAnimation();
}

struct SkillData
{
    public int Id { get; set; }

    public SkillType Type { get; set; }

    public ISkillScene scene { get; set; }

    public float Delay { get; set; }

    public float ConsumeSp { get; set; }
}

public class SkillManager
{
   

}