using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace BilbolStack.CryptoJWT.Chain
{
    [Function("ownerOf", "address")]
    public class OwnerOfFunction : FunctionMessage
    {
        [Parameter("uint256", "nftId")]
        public BigInteger NFTId { get; set; }
    }
}
