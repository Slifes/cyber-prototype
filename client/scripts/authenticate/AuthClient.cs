using Godot;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Auth.Data;

public partial class AuthClient : Node
{
  private const string HOST = "http://localhost:8000";

  private readonly System.Net.Http.HttpClient client = new();

  [Export]
  public string Account;

  [Export]
  private bool _authenticated;

  [Export]
  public string AuthToken;

  [Export]
  public int TokenSelected;

  private SessionMapData _session;

  public SessionMapData Session { get { return _session; } }

  public bool IsAuthenticated()
  {
    return _authenticated;
  }

  public string SessionToken()
  {
    return Session.auth_token;
  }

  public void SetTokenSelected(int id)
  {
    TokenSelected = id;
  }

  public async Task<bool> Login(string address, string signature)
  {
    AuthenticatedData auth = await sendAuthRequest(address, signature);

    if (!auth.token.IsNullOrEmpty())
    {
      AuthToken = auth.token;
      Account = address;
      _authenticated = true;
    }

    return _authenticated;
  }

  public async Task<SessionMapData> CreateSessionMap(int characterId)
  {
    if (!IsAuthenticated())
    {
      return new SessionMapData { };
    }

    _session = await SendCreateSession(characterId);

    return _session;
  }

  async Task<T> SendRequest<T>(string path, object data)
  {
    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

    if (AuthToken != null)
      content.Headers.Add("X-Auth-Server", AuthToken);

    GD.Print("Send: ", JsonConvert.SerializeObject(data));

    var response = await client.PostAsync(HOST + path, content);

    var responseString = await response.Content.ReadAsStringAsync();

    GD.Print("POST: ", responseString);

    return JsonConvert.DeserializeObject<T>(responseString);
  }

  async Task<T> SendRequest<T>(string path)
  {
    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, HOST + path);

    if (AuthToken != null)
      requestMessage.Headers.Add("X-Auth-Server", AuthToken);

    var response = await client.SendAsync(requestMessage);

    var responseString = await response.Content.ReadAsStringAsync();

    GD.Print("GET: ", responseString);

    return JsonConvert.DeserializeObject<T>(responseString);
  }

  private async Task<AuthenticatedData> sendAuthRequest(string address, string signature)
  {
    var data = new
    {
      address,
      signature
    };

    return await SendRequest<AuthenticatedData>("/auth/authenticate/", data);
  }

  public async Task<List<TokenData>> getCharacters()
  {
    return await SendRequest<List<TokenData>>("/auth/tokens/");
  }

  private async Task<SessionMapData> SendCreateSession(int tokenId)
  {
    var data = new
    {
      token = tokenId
    };

    return await SendRequest<SessionMapData>("/auth/session/", data);
  }

  public async Task<CharacterCreateData> SendCreateCharacter(int token_id, string name, string color)
  {
    var data = new
    {
      token = token_id,
      name = name,
      color = color
    };

    return await SendRequest<CharacterCreateData>("/world/character/create/", data);
  }
}
