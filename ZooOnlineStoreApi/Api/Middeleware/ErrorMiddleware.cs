using Microsoft.AspNetCore.WebUtilities;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Middeleware
{
    public class ErrorMiddleware : MiddlewareBase
    {
        public ErrorMiddleware(RequestDelegate next) : base(next) { }

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                //обработка HTTP ошибок
                if (context.Response.StatusCode >= 400 && !context.Response.HasStarted)
                {
                    string message = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode);
                    var error = new//создает JSON-объект
                    {
                        Type = context.Response.StatusCode.ToString(),
                        Message = message
                    };
                    await context.Response.WriteAsJsonAsync(error);
                }
            }
            catch (Exception ex)
            {
                if (ex is DuplicationException dupEx)
                {
                    context.Response.StatusCode = 409;
                    var error = new
                    {
                        Type = "Duplication",
                        Message = dupEx.Message,
                        Field = dupEx.TargetName,
                        Value = dupEx.Value
                    };
                    await context.Response.WriteAsJsonAsync(error);
                    return;
                }

                if (ex is ValidationException valEx)
                {
                    context.Response.StatusCode = 400;
                    var error = new
                    {
                        Type = "Validation",
                        Message = valEx.Message,
                        Field = valEx.TargetName,
                        Value = valEx.Value,
                        Details = valEx.Details
                    };
                    await context.Response.WriteAsJsonAsync(error);
                    return;
                }

                if (ex is UnauthorizedException || ex is InvalidApiKeyException)
                {
                    context.Response.StatusCode = 401;
                    var error = new
                    {
                        Type = "Authentication",
                        Message = ex.Message
                    };
                    await context.Response.WriteAsJsonAsync(error);
                    return;
                }

                if (ex is NotFoundException)
                {
                    context.Response.StatusCode = 404;
                    var error = new
                    {
                        Type = "NotFound",
                        Message = ex.Message
                    };
                    await context.Response.WriteAsJsonAsync(error);
                    return;
                }

                //все остальные исключения - как в вашем исходном коде
                context.Response.StatusCode = 500;
                var genericError = new
                {
                    Type = ex.GetType().Name,
                    Message = ex.Message
                };
                await context.Response.WriteAsJsonAsync(genericError);
            }
        }
    }

}
