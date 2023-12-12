using BilbolStack.CryptoJWT.Chain;
using BilbolStack.CryptoJWT.Common;
using BilbolStack.CryptoJWT.Repository;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BilbolStack.ChainJWT.Common;

namespace BilbolStack.CryptoJWT.Security
{
    public class SecurityManager : ISecurityManager
    {
        private const long TOKEN_SPAN = 60 * 60 * 1000;
        private const string CLAIM_ACCOUNT = "publicAccount";
        private const string CLAIM_NFT = "nft";
        private const string CLAIM_BITS = "bits";
        private const string BAD_SECRET = "thisbelongstoappsettings thisbelongstoappsettings thisbelongstoappsettings";

        private INonceRepository _nonceRepository;
        private IChainValidator _chainValidator;
        private INFTContract _nFTContract;

        public SecurityManager(INonceRepository nonceRepository, IChainValidator chainValidator, INFTContract nFTContract) 
        {
            _nonceRepository = nonceRepository;
            _chainValidator = chainValidator;
            _nFTContract = nFTContract;
        }

        /// <summary>
        /// Gets the challenge message that needs to be signed by an account
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="nftId">The id of the NFT</param>
        /// <returns>The challenge string</returns>
        public string GetChallenge(string account, long nftId)
        {
            if (!_chainValidator.ValidAccount(account))
            {
                throw new BadRequestException($"{account} is not a valid account");
            }

            var nonce = _nonceRepository.Get(account);

            return $"{account} owns {nftId} with nonce {nonce}";
        }

        /// <summary>
        /// Validates the signature to return a JWT token
        /// </summary>
        /// <param name="account">The account</param>
        /// <param name="nftId">The id of the NFT</param>
        /// <param name="signature">The signature signed by the account</param>
        /// <returns>JWT token encapsulating the claims provided by the NFT</returns>
        public async Task<LoginInfo> SignIn(string account, long nftId, string signature)
        {
            if (!_chainValidator.ValidAccount(account))
            {
                throw new BadRequestException($"{account} is not a valid account");
            }

            var challengeResponse = new ChallengeResponse()
            {
                Account = account,
                Message = GetChallenge(account, nftId),
                Signature = signature
            };

            if (!_chainValidator.ValidateSignature(challengeResponse))
            {
                throw new UnAuthorizedAccessException($"Signature is not valid!");
            }

            await ValidateOwnership(account, nftId);

            var bits = await _nFTContract.BitsOfNFT(nftId);

            var expiresOn = DateTime.UtcNow.AddSeconds(TOKEN_SPAN);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(BAD_SECRET);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(CLAIM_ACCOUNT, account), new Claim(CLAIM_BITS, bits.ToString()), new Claim(CLAIM_NFT, nftId.ToString()) }),
                Expires = expiresOn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginInfo() { Account = account, Token = tokenHandler.WriteToken(token), NftId = nftId, Bits = bits };
        }

        public async Task<LoginInfo> AssertAccess(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(BAD_SECRET);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromMinutes(1)
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var account = jwtToken.Claims.First(x => x.Type == CLAIM_ACCOUNT).Value;
            var nftId = long.Parse(jwtToken.Claims.First(x => x.Type == CLAIM_NFT).Value);
            var bits = long.Parse(jwtToken.Claims.First(x => x.Type == CLAIM_BITS).Value);

            await ValidateOwnership(account, nftId);
            await ValidateBits(nftId, bits);

            return new LoginInfo() { Account = account, Token = token, NftId = nftId, Bits = bits };
        }

        // TODO: This throws an exception if nft is not minted yet. Compare against total supply.
        private async Task ValidateOwnership(string account, long nftId)
        {
            var nftOwner = await _nFTContract.OwnerOfNFT(nftId);
            if (nftOwner.ToLower() != account.ToLower())
            {
                throw new UnAuthorizedAccessException($"Invalid NFT id!");
            }
        }

        private async Task ValidateBits(long nftId, long bits)
        {
            var nftBits = await _nFTContract.BitsOfNFT(nftId);
            if (nftBits != bits)
            {
                throw new UnAuthorizedAccessException($"Please log in again");
            }
        }
    }
}
