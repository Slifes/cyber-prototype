using Godot;
using Godot.Collections;

partial class ZoneActor: CharacterBody3D, IActorZone
{
  [Signal]
  public delegate void SkillListEventHandler(Array<int> skillsId);

  [Signal]
  public delegate void ExecuteSkillEventHandler(int skillId, Variant data);

  [Signal]
  public delegate void MoveStartedEventHandler(Vector3 position, Vector3 dir, float speed);

  [Signal]
  public delegate void MoveStoppedEventHandler(Vector3 position);

  Ghosting ghosting;

  SkillHandler skillHandler;

  public override void _Ready()
  {
	ghosting = new Ghosting(this);

	skillHandler = new SkillHandler(this);
  }

  public int GetActorID()
  {
	  return System.Int32.Parse(Name);
  }

  public ActorType GetActorType()
  {
	  return ActorType.Player;
  }

  public Vector3 GetActorPosition()
  {
	throw new System.NotImplementedException();
  }

  public Variant GetData()
  {
	throw new System.NotImplementedException();
  }
}
