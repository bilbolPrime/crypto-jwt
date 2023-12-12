using System.Security.Cryptography;

namespace BilbolStack.CryptoJWT.Utils
{
    public class CryptoRandomAdaptor : IRandomAdapter
    {
        /// <summary>
        /// Generate a random nonce
        /// </summary>
        /// <returns>Returns a random nonce</returns>
        public long RandomNonce()
        {
            return RandomLong(1000_000_000, 2000_000_000);
        }

        /// <summary>
        /// Generate a strong random long
        /// </summary>
        /// <param name="min">Min</param>
        /// <param name="maxIncluded">Max</param>
        /// <returns>Crypto-random long value</returns>
        private long RandomLong(long min, long maxIncluded)
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[sizeof(int)];
                random.GetBytes(byteArray);

                var randomValue = BitConverter.ToInt32(byteArray, 0);

                var result = ((randomValue - min) % (maxIncluded - min + 1) + (maxIncluded - min + 1)) % (maxIncluded - min + 1) + min;
                return result;
            }
        }
    }
}
