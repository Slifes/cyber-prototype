const { ethers } = require("hardhat");

module.exports = async ({getNamedAccounts, deployments}) => {
  const {deploy} = deployments;
  const {deployer} = await getNamedAccounts();

  const tokenDeployment = await deployments.get("HRT");
  const token = await ethers.getContractAt("HRT", tokenDeployment.address);

  await token.mint();
};

module.exports.tags = ['mint'];