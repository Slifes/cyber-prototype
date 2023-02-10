using Godot;
using DialogueManagerRuntime;

class Dialogue : IComponent
{
  Talk actor;

  public Dialogue(Talk actor)
  {
	this.actor = actor;
	actor.ActorClicked += Clicked;
  }

  public void Clicked()
  {
	DialogueManager.ShowExampleDialogueBalloon(actor.Dialogue);
  }

  public void InputHandler(InputEvent @event) { }

  public void Update(float delta) { }
}
