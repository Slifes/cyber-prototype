using Godot;
using System.Collections.Generic;
using System.Linq;

partial class SkillList : Window
{
  PackedScene skillItem = ResourceLoader.Load<PackedScene>("res://ui/skill/skill_item.tscn");

  List<Skill> playerSkills;

  public override void _Ready()
  {
	SkillControl.CreateInstance();

	//Button btn = GetNode<Button>("Button");

	//btn.Pressed += OnButtonPressed;
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

	var chunckedData = ChunkList<SkillItem>(skillItens, 4);

	var index = 0;

	foreach (var c in chunckedData)
	{
	  var hContainer = (HBoxContainer)verticalBox.GetChild(index);
	  var skillIndex = 0;

	  foreach (var skill in c)
	  {
		var skillControl = (Control)hContainer.GetChild(skillIndex);

		var dragHelper = new DragHelper();

		dragHelper.AddData(skill);
		skillControl.AddChild(dragHelper);

		skillIndex++;
	  }

	  index++;
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
