using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class TokenController: ControllerBase
    {
        private readonly JwtService _jwt;

        public TokenController(JwtService jwt)
        {
            _jwt = jwt;
        }
       
        [HttpPost("admin")]
        //передаем ключ в header!
        public async Task<IActionResult> AuthAdminAsync(ApiKeyMessage apiKey)
        {
            try
            {
                string token = await _jwt.GenerateAdminTokenAsync(apiKey.ApiKey);
                // Console.WriteLine(token);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,     // Защита от XSS
                    Secure = true,       // Только HTTPS (в проде)
                    SameSite = SameSiteMode.None, // Защита от CSRF
                    Expires = DateTime.UtcNow.AddDays(7), // 7 дней
                    Path = "/"          // Доступ на всех страницах
                    // Domain = "example.com" // Если нужно на поддоменах
                };
               // Response.Cookies.Delete("userToken");
                HttpContext.Response.Cookies.Append("adminToken", token, cookieOptions);
                // 200
                return Ok(token);
            }
            catch (InvalidApiKeyException ex)
            {
                // 401
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Unauthorized(error);
            }
        }
        [HttpPost("user")]
        //передаем ключ в header!
        public async Task<IActionResult> AuthUserAsync(ApiKeyMessage apiKey)
        {
            try
            {
                string token = await _jwt.GenerateUserTokenAsync(apiKey.ApiKey);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,     // Защита от XSS
                    Secure = true,       // Только HTTPS (в проде)
                    SameSite = SameSiteMode.None, // Защита от CSRF
                    Expires = DateTime.UtcNow.AddDays(7), // 7 дней
                    Path = "/"          // Доступ на всех страницах
                    // Domain = "example.com" // Если нужно на поддоменах
                };
                //Response.Cookies.Delete("adminToken");
                HttpContext.Response.Cookies.Append("userToken", token, cookieOptions);
                // 200
                return Ok(token);
            }
            catch (InvalidApiKeyException ex)
            {
                // 401
                ErrorMessage error = new ErrorMessage(Type: ex.GetType().Name, Message: ex.Message);
                return Unauthorized(error);
            }
        }

    }
}
