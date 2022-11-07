pragma solidity 0.8.17;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@chainlink/contracts/src/v0.8/interfaces/AggregatorV3Interface.sol";

contract HRT is ERC721 {

  uint256 public nonce;

  uint256 public cost;

  AggregatorV3Interface internal priceFeed;

  error MintFailed();

  constructor()
    ERC721("Game Heart", "HRT")
  {
    priceFeed = AggregatorV3Interface(0xd0D5e3DB44DE05E9F294BB0a3bEEaF030DE24Ada);

    cost = 5;
  }

  function mint()
    public
    payable
  {
    int currentPrice = getLatestPrice();

    require(currentPrice > 0);

    uint256 value = cost / uint256(currentPrice);

    if(msg.value < value) {
      revert MintFailed();
    }

    _safeMint(msg.sender, nonce);

    nonce += 1;
  }

  function getLatestPrice() public view returns (int) {
    (
      , 
      int price,
      ,
      , 
    ) = priceFeed.latestRoundData();
    return price;
  }
}