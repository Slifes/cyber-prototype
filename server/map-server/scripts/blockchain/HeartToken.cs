using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace GameServer.scripts.blockchain
{
  [Event("Transfer")]
  public class TransferEvent : IEventDTO
  {
    [Parameter("address", "from", 1, true)]
    public string From { get; set; }

    [Parameter("address", "to", 2, true)]
    public string To { get; set; }

    [Parameter("uint256", "tokenId", 3, true)]
    public BigInteger TokenId { get; set; }
  }
}
