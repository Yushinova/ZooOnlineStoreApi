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
        public async Task<IActionResult> AuthAdminAsync([FromHeader(Name="X-Api-Key")] string apiKey)
        {
            try
            {
                string token = await _jwt.GenerateAdminTokenAsync(apiKey);
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
