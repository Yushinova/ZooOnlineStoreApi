using Microsoft.AspNetCore.Mvc;
using ZooOnlineStoreApi.Api.Jwt;

namespace ZooOnlineStoreApi.Api.Controllers
{
    [Route("api/admin/auth")]
    [ApiController]
    public class TokenController: ControllerBase
    {
        private readonly JwtService _jwt;

        public TokenController(JwtService jwt)
        {
            _jwt = jwt;
        }

        [HttpPost]
        //передаем ключ в header!
        public async Task<IActionResult> AuthAsync([FromHeader(Name="X-Api-Key")] string apiKey)
        {
            try
            {
                string token = await _jwt.GenerateTokenAsync(apiKey);
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
