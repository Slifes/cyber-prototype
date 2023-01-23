using Godot;
using System;
using System.Collections.Generic;

class SkillManager
{
    Dictionary<int, Skill> skills;

    private static SkillManager _instance;

    public static SkillManager Instance { get { return _instance; } }

    public static void CreateInstance()
    {
        _instance = new SkillManager();
    }

    SkillManager()
    {
        skills = new();
    }

    public void Load()
    {
        GD.Print("Loading Skills...");

        var resources = Godot.DirAccess.GetFilesAt("res://resources/skills");

        foreach(var res in resources)
        {
            var skill = ResourceLoader.Load<Skill>(String.Format("res://resources/skills/{0}", res.Replace(".remap", "")));

            skills.Add(skill.ID, skill);

            GD.Print("Skill Loaded ", res);
        }
    }

    public Skill Get(int id)
    {
        if (!skills.ContainsKey(id))
            return null;

        return skills[id];
    }
}