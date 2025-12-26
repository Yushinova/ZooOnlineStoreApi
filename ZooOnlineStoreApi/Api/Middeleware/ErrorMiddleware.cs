using Microsoft.AspNetCore.WebUtilities;
using ZooOnlineStoreApi.Services.Exeptions;

namespace ZooOnlineStoreApi.Api.Middeleware
{
    //public class ErrorMiddleware: MiddlewareBase
    //{
    //    public ErrorMiddleware(RequestDelegate next) : base(next) { }

    //    public override async Task InvokeAsync(HttpContext context)
    //    {
    //        try
    //        {
    //            await _next(context);
    //            // process 4xx (excepting errors by our methods)
    //            int statusCode = context.Response.StatusCode;
    //            if (statusCode / 100 == 4 && !context.Response.HasStarted)
    //            {
    //                string message = ReasonPhrases.GetReasonPhrase(statusCode);
    //                ErrorMessage error = new ErrorMessage(Type: statusCode.ToString(), Message: message);
    //                await context.Response.WriteAsJsonAsync(error);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            // process 500
    //            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //            ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
    //            await context.Response.WriteAsJsonAsync(error);
    //        }
    //    }
    //}

    public class ErrorMiddleware : MiddlewareBase
    {
        private readonly ILogger<ErrorMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorMiddleware(
            RequestDelegate next,
            ILogger<ErrorMiddleware> logger,
            IWebHostEnvironment env) : base(next)
        {
            _logger = logger;
            _env = env;
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Обрабатываем 4xx ошибки только если нет тела
                if (context.Response.StatusCode / 100 == 4 &&
                    !context.Response.HasStarted &&
                    context.Response.ContentLength == 0)
                {
                    string message = ReasonPhrases.GetReasonPhrase(context.Response.StatusCode);
                    ErrorMessage error = new ErrorMessage(
                        Type: context.Response.StatusCode.ToString(),
                        Message: message
                    );
                    await WriteSafeJsonAsync(context, error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                // Определяем статус код для кастомных исключений
                var statusCode = ex switch
                {
                    ValidationException => 400,
                    DuplicationException => 409,
                    NotFoundException => 404,
                    UnauthorizedAccessException => 401,
                    _ => 500
                };

                context.Response.StatusCode = statusCode;

                // Для production скрываем детали 500 ошибок
                var message = (statusCode == 500 && !_env.IsDevelopment())
                    ? "An internal server error occurred"
                    : ex.Message;

                ErrorMessage error = new ErrorMessage(
                    Type: ex.GetType().Name,
                    Message: message
                );

                await WriteSafeJsonAsync(context, error);
            }
        }

        private async Task WriteSafeJsonAsync(HttpContext context, object error)
        {
            // Проверяем, можно ли писать
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Cannot write error response - response already started");
                return;
            }

            // Сбрасываем буфер
            context.Response.Clear();

            // Устанавливаем Content-Type
            context.Response.ContentType = "application/json";

            // Записываем
            await context.Response.WriteAsJsonAsync(error);
        }
    }
}
