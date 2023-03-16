using Godot;
using Godot.Collections;

partial class CharacterSelector : Control
{
  AuthClient client;

  public override void _Ready()
  {
    client = GetNode<AuthClient>("/root/AuthClient");
  }

  public async void Load(Control target)
  {
    var tokens = await client.getCharacters();

    var characterArray = new Array();

    foreach (var token in tokens)
    {
      Dictionary obj = new();

      obj.Add("id", token.id);
      obj.Add("token_id", token.token_id);

      if (token.character != null)
      {
        Dictionary character_data = new();

        character_data.Add("name", token.character.name);
        character_data.Add("color", token.character.color);

        obj.Add("character", character_data);
      }

      characterArray.Add(obj);
    }

    target.CallDeferred("emit_signal", "_receive_characters", characterArray);
  }

  public async void CreateSession(int characterId, Control target)
  {
    var session = await client.CreateSessionMap(characterId);

    if (session.auth_token != null)
    {
      target.CallDeferred("emit_signal", "_session_map_created", session.auth_token);
    }
  }
}
