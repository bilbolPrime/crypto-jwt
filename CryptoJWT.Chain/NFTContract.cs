using BilbolStack.ChainJWT.Common;
using Microsoft.Extensions.Options;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;

namespace BilbolStack.CryptoJWT.Chain
{
    public class NFTContract : INFTContract
    {
        protected Web3 _web3;
        protected string _contractAddress;
        protected Account _account;

        public NFTContract(IOptions<ChainSettings> chainSettings)
        {
            _account = new Account(chainSettings.Value.AccountPrivateKey, chainSettings.Value.ChainId);
            _web3 = new Web3(_account, chainSettings.Value.RpcUrl);
            _web3.TransactionManager.UseLegacyAsDefault = true;
            _contractAddress = chainSettings.Value.NFTContractAddress;
        }

        public async Task<long> BitsOfNFT(long nftId)
        {
            var function = new NFTBitsFunction() { NFTId = new BigInteger(nftId) };
            return await _web3.Eth.GetContractQueryHandler<NFTBitsFunction>()
                  .QueryAsync<long>(_contractAddress, function);
        }

        public async Task<string> OwnerOfNFT(long nftId)
        {
            var function = new OwnerOfFunction() { NFTId = new BigInteger(nftId) };
            return await _web3.Eth.GetContractQueryHandler<OwnerOfFunction>()
                  .QueryAsync<string>(_contractAddress, function);
        }
    }
}
