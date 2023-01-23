using Godot;
using System.Collections.Generic;
using System.Linq;

partial class SkillList: Control
{
  PackedScene skillItem = ResourceLoader.Load<PackedScene>("res://ui/skill/skill_item.tscn");

  List<Skill> playerSkills;

  Panel panel;

  public override void _Ready()
  {
	Button btn = GetNode<Button>("Button");

	panel = GetNode<Panel>("Panel");

	btn.Pressed += OnButtonPressed;
  }

  void OnButtonPressed()
  {
	panel.Visible = !panel.Visible;
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

	var verticalBox = GetNode<VBoxContainer>("Panel/ScrollContainer/MarginContainer/VBoxContainer");

	var chunckedData = ChunkList<SkillItem>(skillItens, 4);

	foreach(var c in chunckedData)
	{
	  var hContainer = new HBoxContainer();
	  verticalBox.AddChild(hContainer);
	  hContainer.SizeFlagsHorizontal = SizeFlags.Fill;
	  hContainer.SizeFlagsVertical = SizeFlags.ExpandFill;

	  foreach(var skill in c)
	  {
		hContainer.AddChild(skill); 
	  }
	}
  }

  public static List<List<T>> ChunkList<T>(List<T> data, int size)
  {
	return data
	  .Select((x, i) => new { Index = i, Value = x })
	  .GroupBy(x => x.Index / size)
	  .Select(x => x.Select(v => v.Value).ToList())
	  .ToList();
  }
}
