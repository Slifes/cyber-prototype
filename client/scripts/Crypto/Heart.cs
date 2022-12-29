using System;
using System.Threading.Tasks;
using Godot;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Teste.scripts.Crypto
{
	public class Heart
	{
		Contract contract;

		const String CONTRACT_ADDRESS = "0xEbe678C8389bC74Ed1A7BD91407A03E96BAAB1bD";

		public Heart(Web3 web3)
		{
			var abi = Godot.FileAccess.Open("res://abi/hrt.json", Godot.FileAccess.ModeFlags.Read);
			contract = web3.Eth.GetContract(abi.GetAsText(), CONTRACT_ADDRESS);
			abi.Dispose();
		}

		public async Task<int> GetCryptoPrice()
		{
			GD.Print("Get");
			Function latestPrice = contract.GetFunction("getLatestPrice");

			var price = await latestPrice.CallAsync<int>(new object[0]);

			GD.Print(price);

			return price;
		}

		public async Task<String> AsyncMintHeart(String account)
		{
			int price = await GetCryptoPrice();

			GD.Print("Price: ", price);

			Function mint = contract.GetFunction("mint");

			HexBigInteger gas = await mint.EstimateGasAsync(account, new HexBigInteger("0x0"), new HexBigInteger(price * 5), Array.Empty<object>());

			String txId = await mint.SendTransactionAsync(account, gas, new HexBigInteger(price * 5), Array.Empty<object>());

			return txId;
		}

		public void MintHeart(Node targetTo, String account)
		{
			GD.Print("MintHeart");

			AsyncMintHeart(account).ContinueWith(t =>
			{
				targetTo.EmitSignal("_mint_txsended", t.Result);
			});
		}
	}
}
