const { deployProxy, upgradeProxy } = require('@openzeppelin/truffle-upgrades');
var NFTContract = artifacts.require('NFTContract');
module.exports = async function (deployer, network, accounts) {
  await deployProxy(NFTContract, [], { deployer });
};