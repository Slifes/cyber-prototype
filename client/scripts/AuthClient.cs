using Godot;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Godot.Collections;

public partial class AuthClient: Node2D
{
	public struct AuthenticatedData
	{
		public string token;
		public string expire_at;
	}

	public struct CharacterData
	{
		public string id;
		public string token_id;
	}

	public struct SessionMapData
	{
		public string auth_token;
		public string expire_at;
	}

	private string _address;

	private bool _authenticated;
	
	private string _authToken;

	private readonly HttpClient client = new();

	private SessionMapData _session;

	public bool IsAuthenticated { get { return _authenticated; } }
	
	public string Account { get { return _address; } }

	public SessionMapData Session { get { return _session;  } }

	public string SessionToken()
	{
		return Session.auth_token;
	}

	public async Task<bool> Connect(string address, string signature)
	{
		AuthenticatedData auth = await sendAuthRequest(address, signature);

		if (!auth.token.IsNullOrEmpty())
		{
			_authToken = auth.token;
			_address = address;
			_authenticated = true;
		}

		return _authenticated;
	}

	public async void GetCharacters(Node2D target)
	{
		if (!IsAuthenticated)
		{
			return;
		}

		var characters = await getCharacters();

		var characterArray = new Array();
		
		foreach (var character in characters)
		{
			Dictionary obj = new();

            obj.Add("id", character.id);
            obj.Add("token_id", character.token_id);

			characterArray.Add(Variant.CreateFrom(obj));
		}

		target.CallDeferred("emit_signal", "_receive_characters", characterArray);
	}

	public async void CreateSessionMap(int characterId, Node2D target)
	{
		if (!IsAuthenticated)
		{
			return;
		}

		_session = await sendSessionRequest(characterId);

		target.CallDeferred("emit_signal", "_session_map_created", _session.auth_token);
	}

	private async Task<AuthenticatedData> sendAuthRequest(string address, string signature)
	{
		var data = new
		{
			address,
			signature
		};

		var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

		var response = await client.PostAsync("http://localhost:8000/auth/authenticate/", content);

		var responseString = await response.Content.ReadAsStringAsync();

		return JsonConvert.DeserializeObject<AuthenticatedData>(responseString);
	}

	private async Task<List<CharacterData>> getCharacters()
	{
		using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8000/auth/tokens/");

		requestMessage.Headers.Add("X-Auth-Server", _authToken);

		var response = await client.SendAsync(requestMessage);

		var responseString = await response.Content.ReadAsStringAsync();

		GD.Print(responseString);

		return JsonConvert.DeserializeObject<List<CharacterData>>(responseString);
	}

	private async Task<SessionMapData> sendSessionRequest(int characterId)
	{
		var data = new
		{
            character = characterId
		};

		var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

		content.Headers.Add("X-Auth-Server", _authToken);

		var response = await client.PostAsync("http://localhost:8000/auth/session/", content);

		var responseString = await response.Content.ReadAsStringAsync();

		GD.Print(responseString);

		return JsonConvert.DeserializeObject<SessionMapData>(responseString);
	}
}
