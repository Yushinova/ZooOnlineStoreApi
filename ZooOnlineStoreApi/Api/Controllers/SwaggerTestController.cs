using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // Разрешаем доступ без авторизации
    public class SwaggerTestController : ControllerBase
    {
        private readonly JwtService _jwtService;

        // Тестовые API ключи (можно вынести в appsettings)
        private const string TEST_ADMIN_API_KEY = "$2a$12$ncjskFMRG08WaoGrZkXhGejk1tEouvdq8NGY3P4BjsJglEv3MIgN6";
        private const string TEST_USER_API_KEY = "$2a$12$ncjskFMRG08WaoGrZkXhGeEw.gee0RRyvigA8IpUcg1eGENqkNlVu";

        public SwaggerTestController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Get a test administrator token for Swagger
        /// </summary>
        /// <remarks>
        /// Uses a pre-installed test API key.
        /// Returns a token in the response body for use in Swagger UI.
        /// </remarks>
        [HttpPost("admin-token")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTestAdminToken()
        {
            try
            {
                // Используем тестовый ключ администратора
                string token = await _jwtService.GenerateAdminTokenAsync(TEST_ADMIN_API_KEY);

                // Устанавливаем HttpOnly куку (как в основном контроллере)
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Path = "/"
                };

                // Удаляем userToken если был
                Response.Cookies.Delete("userToken");

                // Устанавливаем adminToken
                Response.Cookies.Append("adminToken", token, cookieOptions);

                // Возвращаем токен в теле для Swagger UI
                return Ok(new
                {
                    Token = token,
                    Message = "Тестовый токен администратора успешно создан",
                    Instructions = "Скопируйте значение Token и используйте в Swagger UI",
                    Usage = "В Swagger UI нажмите 'Authorize' и введите: Bearer [Token]",
                    Role = JwtService.ADMIN_ROLE,
                    ExpiresIn = "7 дней"
                });
            }
            catch (InvalidApiKeyException ex)
            {
                // Если тестовый ключ не работает
                ErrorMessage error = new ErrorMessage(
                    Type: ex.GetType().Name,
                    Message: "Тестовый API ключ администратора недействителен. Проверьте базу данных."
                );
                return Unauthorized(error);
            }
        }

        /// <summary>
        /// Get a test user token for Swagger
        /// </summary>
        [HttpPost("user-token")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorMessage), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetTestUserToken()
        {
            try
            {
                // Используем тестовый ключ пользователя
                string token = await _jwtService.GenerateUserTokenAsync(TEST_USER_API_KEY);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7),
                    Path = "/"
                };

                // Удаляем adminToken если был
                Response.Cookies.Delete("adminToken");

                // Устанавливаем userToken
                Response.Cookies.Append("userToken", token, cookieOptions);

                return Ok(new
                {
                    Token = token,
                    Message = "Тестовый токен пользователя успешно создан",
                    Instructions = "Скопируйте значение Token и используйте в Swagger UI",
                    Usage = "В Swagger UI нажмите 'Authorize' и введите: Bearer [Token]",
                    Role = JwtService.USER_ROLE,
                    ExpiresIn = "7 дней"
                });
            }
            catch (InvalidApiKeyException ex)
            {
                ErrorMessage error = new ErrorMessage(
                    Type: ex.GetType().Name,
                    Message: "Тестовый API ключ пользователя недействителен. Проверьте базу данных."
                );
                return Unauthorized(error);
            }
        }

        /// <summary>
        /// Get information about the current token
        /// </summary>
        [HttpGet("token-info")]
        public IActionResult GetTokenInfo()
        {
            var adminToken = Request.Cookies["adminToken"];
            var userToken = Request.Cookies["userToken"];

            return Ok(new
            {
                HasAdminToken = !string.IsNullOrEmpty(adminToken),
                HasUserToken = !string.IsNullOrEmpty(userToken),
                AdminTokenLength = adminToken?.Length ?? 0,
                UserTokenLength = userToken?.Length ?? 0,
                Message = "Эта информация только для отладки"
            });
        }

        /// <summary>
        /// Delete all tokens (exit)
        /// </summary>
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("adminToken");
            Response.Cookies.Delete("userToken");

            return Ok(new
            {
                Message = "Все токены удалены",
                Instructions = "Куки очищены, требуется повторная авторизация"
            });
        }
    }
}
