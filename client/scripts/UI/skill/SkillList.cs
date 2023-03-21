using Godot;
using System.Collections.Generic;

partial class SkillList : Window
{
  PackedScene skillItem = ResourceLoader.Load<PackedScene>("res://ui/skill/skill_item.tscn");

  List<Skill> playerSkills;

  public override void _Ready()
  {
    SkillControl.CreateInstance();
  }

  public void SetSkills(List<Skill> skills)
  {
    playerSkills = skills;

    CreateSkillList();
  }

  List<SkillItem> CreateSkillItens()
  {
    var skills = new List<SkillItem>(new SkillItem[playerSkills.Count]);

    for (var i = 0; i < playerSkills.Count; i++)
    {
      skills[i] = skillItem.Instantiate<SkillItem>();
      skills[i].skill = playerSkills[i];
    }

    return skills;
  }

  void CreateSkillList()
  {
    var skillItens = CreateSkillItens();

    var verticalBox = GetNode<VBoxContainer>("ScrollContainer/MarginContainer/VBoxContainer");

    var flow = GetNode<HFlowContainer>("ScrollContainer/MarginContainer/VBoxContainer/HFlowContainer");
    //var chunckedData = ChunkList<SkillItem>(skillItens, 4);

    foreach (var skill in skillItens)
    {
      flow.AddChild(DragHelper.Create(skill, new Vector2(85, 85)));
    }
  }
}
