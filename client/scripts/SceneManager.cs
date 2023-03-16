using Godot;

public partial class SceneManager : Node
{
  enum State
  {
    Authenticate,
    CharacterCreate,
    World
  }

  private State currentState;

  public string GetCurrentState()
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
    else if (state == "character_creator")
    {
      return State.CharacterCreate;
    }

    return State.Authenticate;
  }

  public void ChangeState(string state)
  {
    var _state = GetStateByString(state);

    switch (_state)
    {
      case State.Authenticate:
        GetTree().ChangeSceneToFile("res://scenes/intro.tscn");
        break;

      case State.World:
        GetTree().ChangeSceneToFile("res://scenes/world.tscn");
        break;

      case State.CharacterCreate:
        GetTree().ChangeSceneToFile("res://scenes/character_creation.tscn");
        break;
    }

    currentState = _state;
  }
}
