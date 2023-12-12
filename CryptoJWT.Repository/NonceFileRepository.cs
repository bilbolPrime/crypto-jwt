using Newtonsoft.Json;
using BilbolStack.CryptoJWT.Utils;

namespace BilbolStack.CryptoJWT.Repository
{
    public class NonceFileRepository : INonceRepository
    {
        private const string _file = "nonces.txt";
        private const long _grace = 300;
        private static object _lockObj = new object();

        private IRandomAdapter _randomAdapter;

        public NonceFileRepository(IRandomAdapter randomAdapter)
        {
            _randomAdapter = randomAdapter;
        }

        /// <summary>
        /// Gets a nonce for an account.
        /// The nonce Has a lifespan of 5 minutes.
        /// </summary>
        /// <param name="account">Public account</param>
        /// <returns>Nonce to be signed or expected to be signed</returns>
        public long Get(string account)
        {
            lock (_lockObj)
            {
                account = account.ToLower();
                var nonces = File.Exists(_file) ? 
                            JsonConvert.DeserializeObject<List<AccountNonce>>(File.ReadAllText(_file)) 
                            : 
                            new List<AccountNonce>();

                var bingo = nonces.FirstOrDefault(i => i.Account == account);
                if(bingo != null && DateTime.UtcNow.Subtract(bingo.CreatedOn).TotalSeconds < _grace)
                {
                    return bingo.Nonce;
                }

                if(bingo == null)
                {
                    bingo = new AccountNonce()
                    {
                        Account = account
                    };

                    nonces.Add(bingo);
                }

                bingo.Nonce = _randomAdapter.RandomNonce();
                bingo.CreatedOn = DateTime.UtcNow;

                File.WriteAllText(_file, JsonConvert.SerializeObject(nonces));

                return bingo.Nonce;
            }
        }

        private class AccountNonce
        {
            public string? Account { get; set; }
            public long Nonce { get; set; }
            public DateTime CreatedOn { get; set; }
        }
    }
}
