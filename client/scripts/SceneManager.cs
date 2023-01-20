using Godot;
using System;

public partial class SceneManager : Node
{
  enum State
  {
	Authenticate,
	CharacterCreate,
	World
  }

  private State currentState;

  public Variant GetCurrentState()
  {
	return currentState.ToString();
  }

  private static State GetStateByString(string state)
  {
	if (state == "world")
	{
	  return State.World;
	}
	else if (state == "authenticate")
	{
	  return State.Authenticate;
	}

	return State.Authenticate;
  }

  public void ChangeState(Variant state)
  {
	var _state = GetStateByString(state.ToString());

	if (_state == State.Authenticate)
	{
	  GetTree().ChangeSceneToFile("res://scenes/intro.tscn");
	} else if (_state == State.World)
	{
	  GetTree().ChangeSceneToFile("res://scenes/world.tscn");
	}

	currentState = _state;
  }
}
