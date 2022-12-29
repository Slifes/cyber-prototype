const { expect } = require("chai");

describe("Heart", function(){
  let signer, token;

  before(async function() {
    ;[signer] = await ethers.getSigners();

    const HRT = await ethers.getContractFactory('HRT')
    const tokenDeployed = await HRT.deploy();

    token = await tokenDeployed.deployed();
  });

  it("Should mint a heart", async function () {
    await expect(token.mint({ value: "200000000000000" }))
    .to.emit(token, 'Transfer')
      .withArgs(
        "0x0000000000000000000000000000000000000000",
        signer.address,
        '0'
      );
  });
})