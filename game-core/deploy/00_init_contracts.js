const { ethers } = require("hardhat");

module.exports = async ({getNamedAccounts, deployments}) => {
  const {deploy} = deployments;
  const {deployer} = await getNamedAccounts();

  console.log("---- DEPLOYING CONTRACTS -----");

  let token = await deploy('HRT', {
    from: deployer,
    args: []
  });

  console.log("Token deployed: ", token.address);
};

module.exports.tags = ['init'];