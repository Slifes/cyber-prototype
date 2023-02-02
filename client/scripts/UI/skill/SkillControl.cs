using System.Collections.Generic;
using System.Linq;

class SkillControl
{
  List<SkillItem> skills;

  private static SkillControl _instance;

  public static SkillControl Instance { get { return _instance; } }

  public static void CreateInstance()
  {
    _instance = new SkillControl();
  }

  SkillControl()
  {
    skills = new();
  }

  public void Add(SkillItem item)
  {
    skills.Add(item);
  }

  public void UpdateSkillItems(int ID, int timestamp)
  {
    var items = skills.FindAll(x => x.skill.ID == ID).ToList();

    items.ForEach(x => x.Used());
  }

  public void Remove(SkillItem item)
  {
    skills.Remove(item);
  }
}
