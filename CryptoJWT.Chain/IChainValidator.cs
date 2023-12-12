using BilbolStack.CryptoJWT.Common;

namespace BilbolStack.CryptoJWT.Chain
{
    public interface IChainValidator
    {
        bool ValidAccount(string address);
        bool ValidateSignature(ChallengeResponse data);
    }
}
