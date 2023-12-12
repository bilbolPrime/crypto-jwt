namespace BilbolStack.CryptoJWT.Repository
{
    public interface INonceRepository
    {
        long Get(string account);
    }
}
