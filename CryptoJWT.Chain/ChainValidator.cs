using BilbolStack.CryptoJWT.Common;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;

namespace BilbolStack.CryptoJWT.Chain
{
    public class ChainValidator : IChainValidator
    {
        /// <summary>
        /// Check if the address is valid
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>True if the address is valid 20 byte ethereum address</returns>
        public bool ValidAccount(string address)
        {
            if (string.IsNullOrEmpty(address) || address.Length != 42)
            {
                return false;
            }

            return new AddressUtil().IsChecksumAddress(address);
        }

        /// <summary>
        /// Validates a signature
        /// </summary>
        /// <param name="data">Account, message and the signature of the message</param>
        /// <returns>True if the signature is correct</returns>
        public bool ValidateSignature(ChallengeResponse data)
        {
            var signer = new EthereumMessageSigner();
            var bytes = System.Text.Encoding.UTF8.GetBytes(data.Message);
            var hash = BitConverter.ToString(bytes).Replace("-", "");
            return data.Account == signer.EcRecover(hash.HexToByteArray(), data.Signature);
        }
    }
}
