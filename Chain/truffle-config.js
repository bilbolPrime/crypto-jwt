require('dotenv').config();

function hdWalletProviderOptions(privateKeyEnvVarValue, mnemonicPhraseEnvVarValue, otherOpts) {
    const opts = {
        ...otherOpts
    };
    if (privateKeyEnvVarValue) {
        opts.privateKeys = [privateKeyEnvVarValue];
    } else {
        opts.mnemonic = mnemonicPhraseEnvVarValue;
    }
    return opts;
}

const HDWalletProvider = require('@truffle/hdwallet-provider');

module.exports = {
    networks: {
        //
        development: {
            host: "127.0.0.1", // Localhost (default: none)
            port: 7545, // Standard Ethereum port (default: none)
            network_id: "*", // Any network (default: none)
        }
	},
     compilers: {
            solc: {
                version: "0.8.6"
            }
    }
};
