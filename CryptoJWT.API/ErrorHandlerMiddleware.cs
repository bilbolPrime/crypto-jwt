using BilbolStack.ChainJWT.Common;
using System.Text.Json;

namespace BilbolStack.CryptoJWT.API
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string message = string.Empty;
            bool success = false;
            try
            {
                await _next(context);
                success = true;
            }
            catch (UnAuthorizedAccessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                message = ex.Message;
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                message = ex.Message;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                message = "oops";
            }

            if (!success)
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    IgnoreNullValues = true
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse() { Message = message }, options));
            }
        }
    }
}
