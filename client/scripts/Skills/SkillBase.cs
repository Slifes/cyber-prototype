using System;

public class SkillBase 
{
	enum SkillAttributes
	{
		Normal,
		Fire,
		Freeze,
		Water,
		Wealth
	}

	struct SkillAttribute
	{
		SkillAttributes attribute;
		float value;
	}

	enum SkillTrigger
	{
		Shoot,
		Invoke,
		Spawn,

	}

	public int ID { get; }

	public int Level { get; }

    public Array attributes;

}
