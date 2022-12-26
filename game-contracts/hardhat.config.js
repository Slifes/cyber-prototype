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
  'df3e90a7e37fe9b948bcb92531d743b6f17df24af78493ac3ab7d0935238b68d'
const oraclePrivateKey =
  process.env.ORACLE_PRIVATE_KEY ||
  'e2a7a378c6c6dff822361ab053e4bd2647a9fed6f0e94008d10af7decb90d956'

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
      accounts: [platformPrivateKey, oraclePrivateKey],
      tags: ["staging"],
    },
  },
  namedAccounts: {
    deployer: {
      default: '0x70B790EAb258d82E18BB440978D84feF2dfE3F7B',
    },
    oracle: {
      default: '0xabEfE9cd95bC0b2d8E81c5cAdED024a6b121834a',
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