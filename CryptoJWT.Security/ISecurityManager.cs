using BilbolStack.CryptoJWT.Common;

namespace BilbolStack.CryptoJWT.Security
{
    public interface ISecurityManager
    {
        string GetChallenge(string account, long nftId);
        Task<LoginInfo> SignIn(string account, long nftId, string signature);
        Task<LoginInfo> AssertAccess(string token);
    }
}
