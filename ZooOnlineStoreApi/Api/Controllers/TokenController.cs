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
        public async Task<IActionResult> AuthAdminAsync([FromBody] string apiKey)
        {
            try
            {
                string token = await _jwt.GenerateAdminTokenAsync(apiKey);
                // Console.WriteLine(token);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,     // Защита от XSS
                    Secure = true,       // Только HTTPS (в проде)
                    SameSite = SameSiteMode.None, // Защита от CSRF
                    Expires = DateTime.UtcNow.AddDays(30), // Долгий срок
                    Path = "/"          // Доступ на всех страницах
                    // Domain = "example.com" // Если нужно на поддоменах
                };
                HttpContext.Response.Cookies.Append("adminApiKey", apiKey, cookieOptions);
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
        public async Task<IActionResult> AuthUserAsync([FromHeader(Name = "X-Api-Key")] string apiKey)
        {
            try
            {
                string token = await _jwt.GenerateUserTokenAsync(apiKey);
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
