using Godot;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.Hex.HexTypes;
using Nethereum.BlockchainProcessing;


namespace GameServer.scripts.blockchain
{
  public class Chain
  {
    class SyncState
    {
      public uint LastBlockVerified { get; set; }
      public uint CurrentBlockNumber { get; set; }
    }

    Web3 web3;

    BlockchainProcessor processor;

    List<EventLog<TransferEvent>> events = new List<EventLog<TransferEvent>>();

    SyncState syncState;

    const string CONTRACT_ADDRESS = "0xEbe678C8389bC74Ed1A7BD91407A03E96BAAB1bD";
    const string HTTP_PROVIDER = "https://rpc-mumbai.maticvigil.com/";

    public Chain()
    {
      web3 = new Web3(HTTP_PROVIDER);

      syncState = new SyncState();

      web3.Eth.Blocks.GetBlockNumber.SendRequestAsync().ContinueWith(t =>
      {
        syncState.LastBlockVerified = (uint)t.Result.ToUlong() - 1;
        syncState.CurrentBlockNumber = (uint)t.Result.ToUlong();
      });

      events = new List<EventLog<TransferEvent>>();

      processor = web3.Processing.Logs.CreateProcessorForContract<TransferEvent>(
          CONTRACT_ADDRESS,
          action: ev => events.Add(ev)
      );
    }

    public async Task<bool> Pool()
    {
      var t = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

      syncState.CurrentBlockNumber = (uint)t.ToUlong();

      CancellationToken cancellationToken = new CancellationToken();

      if (syncState.LastBlockVerified < syncState.CurrentBlockNumber)
      {
        await processor.ExecuteAsync(
            toBlockNumber: syncState.CurrentBlockNumber,
            cancellationToken: cancellationToken,
            startAtBlockNumberIfNotProcessed: syncState.LastBlockVerified);

        GD.Print("Block from: ", syncState.LastBlockVerified);
        GD.Print("Block to: ", syncState.CurrentBlockNumber);
        GD.Print("Events count: ", events.Count);

        syncState.LastBlockVerified = syncState.CurrentBlockNumber;
      }

      foreach (var e in events)
      {
        GD.Print("--------------- New NFT minted ---------------");
        GD.Print("From: ", e.Event.From);
        GD.Print("To: ", e.Event.To);
        GD.Print("TokenID: ", e.Event.TokenId);
      }

      events.Clear();

      return true;
    }

    private Contract GetHeartContract()
    {
      var abi = FileAccess.Open("res://abi/hrt.json", FileAccess.ModeFlags.Read);

      return web3.Eth.GetContract(abi.GetAsText(), CONTRACT_ADDRESS);
    }
  }
}
