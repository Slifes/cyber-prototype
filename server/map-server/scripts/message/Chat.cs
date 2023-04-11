using System.Collections.Generic;

enum ChatType
{
  Global,
  Private,
}

struct Message
{
  public int ID;
  public string Text;
  public string Sender;
  public long Timestamp;
}

class Room
{
  ChatType Type;

  public List<Message> Messages;

  public void AddMessage(SessionActor actor, string message)
  {
    Messages.Add(new Message
    {
      ID = actor.GetActorId(),
      Text = message,
      Sender = actor.Name,
      Timestamp = (long)System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds
    });
  }
}
