using BilbolStack.CryptoJWT.Security;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BilbolStack.CryptoJWT.API
{
    public class NFTActionFilter : IActionFilter
    {
        private readonly ISecurityManager _securityManager;
        public NFTActionFilter(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            _securityManager.AssertAccess(token);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
}
