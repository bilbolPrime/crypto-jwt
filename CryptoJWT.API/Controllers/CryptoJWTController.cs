using BCC = BilbolStack.CryptoJWT.Common;
using BilbolStack.CryptoJWT.Security;
using BilbolStack.CryptoJWT.API;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace BilbolStack.CryptoJWT.Controllers
{
    [ApiController]
    [Route("/")]
    public class CryptoJWTController : ControllerBase
    {
        private ISecurityManager _securityManager;
        public CryptoJWTController(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        [HttpPost("challenge")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ChallengeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public ChallengeResponse Get(ChallengeRequest challengeRequest)
        {
            var message = _securityManager.GetChallenge(challengeRequest.Account, challengeRequest.NFTId);
            return new ChallengeResponse() { Message = message };
        }

        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(BCC.LoginInfo), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<BCC.LoginInfo> Login(LoginRequest loginRequest)
        {
            var loginInfo = await _securityManager.SignIn(loginRequest.Account, loginRequest.NFTId, loginRequest.Signature);
            return loginInfo;
        }

        [HttpGet("test")]
        [ServiceFilter(typeof(NFTActionFilter))]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task Test([FromHeader(Name = "Authorization")][Required] string authorization)
        {
            
        }
    }
}
