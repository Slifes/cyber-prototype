using Godot;

partial class BaseNpcActor : CharacterActor
{
  [Signal]
  public delegate void SvChangeStateEventHandler(Variant state, Variant position, Variant yaw, Variant data);

  // [Signal]
  // public delegate void Update

  public int ID;

  public AnimationPlayer Animation { get; set; }

  public override void _Ready()
  {
    components = new IComponent[0];

    SetProcessUnhandledInput(false);
    SetProcessInput(false);
    SetProcessShortcutInput(false);
  }

  // public void ReceiveChangeState(Variant state, Variant position, Variant yaw, Variant data)
  // {
  // 	AIState _state = (AIState)(int)state;

  // 	GlobalPosition = (Vector3)position;
  // 	RotationDegrees = new Vector3(0, (float)yaw, 0);

  // 	ChangeState(_state, data);
  // }

  // public void ReceiveUpdateState(Variant state, Variant position, Variant yaw, Variant data)
  // {
  // 	GlobalPosition = (Vector3)position;
  // 	RotationDegrees = new Vector3(0, (float)yaw, 0);

  // 	if (behavior != null)
  // 	{
  // 		behavior.SetData(data);
  // 	}
  // }
}
