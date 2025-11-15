using Microsoft.AspNetCore.WebUtilities;

namespace ZooOnlineStoreApi.Api.Middeleware
{
    public class ErrorMiddleware: MiddlewareBase
    {
        public ErrorMiddleware(RequestDelegate next) : base(next) { }

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                // process 4xx (excepting errors by our methods)
                int statusCode = context.Response.StatusCode;
                if (statusCode / 100 == 4 && !context.Response.HasStarted)
                {
                    string message = ReasonPhrases.GetReasonPhrase(statusCode);
                    ErrorMessage error = new ErrorMessage(Type: statusCode.ToString(), Message: message);
                    await context.Response.WriteAsJsonAsync(error);
                }
            }
            catch (Exception ex)
            {
                // process 500
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                await context.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
