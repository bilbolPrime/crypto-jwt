namespace BilbolStack.CryptoJWT.Chain
{
    public interface INFTContract
    {
        Task<string> OwnerOfNFT(long nftId);
        Task<long> BitsOfNFT(long nftId);
    }
}
