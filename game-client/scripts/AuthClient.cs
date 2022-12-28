using Godot;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

public partial class AuthClient: Node2D
{
	struct AuthenticateData
	{
		public string address;
		public string signature;
	}

	public struct AuthenticatedData
	{
		public string token;
		public string expire_at;
	}

	private string address;

	private string authToken;

	public async Task<AuthenticatedData> SendAuthRequest(string address, string signature)
	{
		using (var client = new HttpClient())
		{
			var data = new
			{
				address = address,
				signature = signature
			};

			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

			var response = await client.PostAsync("http://localhost:8000/auth/authenticate/", content);

			var responseString = await response.Content.ReadAsStringAsync();

			GD.Print(responseString);

			return JsonConvert.DeserializeObject<AuthenticatedData>(responseString);
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
