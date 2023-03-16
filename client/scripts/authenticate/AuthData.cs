
namespace Auth.Data
{
  public struct AuthenticatedData
  {
    public string token;
    public string expire_at;
  }

  public class CharacterData
  {
    public string name;

    public string color;
  }

  public struct TokenData
  {
    public int id;
    public int token_id;
    public CharacterData character;
  }

  public struct CharacterCreateData
  {
    public int token;
    public string name;
    public string color;
  }

  public struct SessionMapData
  {
    public string auth_token;
    public string expire_at;
  }
}
