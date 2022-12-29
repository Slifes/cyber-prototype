require('@nomiclabs/hardhat-waffle')
require('hardhat-gas-reporter')
require('hardhat-deploy')
require('solidity-coverage');

// This is a sample Hardhat task. To learn how to create your own go to
// https://hardhat.org/guides/create-task.html
task('accounts', 'Prints the list of accounts', async () => {
  const accounts = await ethers.getSigners()

  for (const account of accounts) {
    console.log(account.address)
  }
})

// You need to export an object to set up your config
// Go to https://hardhat.org/config/ to learn more

const platformPrivateKey =
  process.env.PRIVATE_KEY ||
  '274574b211915960436709a6f032d258106d4fa9d63b35a8158598da4f00dedc'

/**
 * @type import('hardhat/config').HardhatUserConfig
 */
module.exports = {
  defaultNetwork: 'mumbai',
  networks: {
    mumbai: {
      live: true,
      url: "https://rpc-mumbai.maticvigil.com",
      saveDeployments: true,
      accounts: [platformPrivateKey],
      tags: ["staging"],
    },
  },
  namedAccounts: {
    deployer: {
      default: '0x2b9d37c2B65B33ea2D081879dfB12A842F2C7506',
    },
  },
  solidity: {
    compilers: [
      {
        version: '0.8.17',
        settings: {
          optimizer: {
            enabled: true,
            runs: 200,
          },
        },
      },
    ],
  },
  gasReporter: {
    enabled: true,
    currency: 'USD',
    gasPrice: 1,
    coinmarketcap: 'ccb040bd-9fb3-45cf-a6cc-2fb21330a82c',
  },
}