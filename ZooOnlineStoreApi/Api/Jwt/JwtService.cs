using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZooOnlineStoreApi.Model.Admins;
using ZooOnlineStoreApi.Model.Exeptions;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.Jwt
{
    public class JwtService
    {
        //параметры jwt-схемы и токена
        private const string JWT_ISSUER = "ZooOnlineStoreApi_issuer";
        private const string JWT_AUDIENCE = "ZooOnlineStoreApi_audience";
        private const int JWT_LIFE_TIME_MINUTES = 30;

        // ConfigureJwtOptions - метод конфигурации jwt-схемы аутентификации
        public static void ConfigureJwtOptions(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                // издатель токена и валидировать ли издателя токена
                ValidateIssuer = true,
                ValidIssuer = JWT_ISSUER,
                // потребитель токена и валидировать ли потребителя токена
                ValidateAudience = true,
                ValidAudience = JWT_AUDIENCE,
                // параметры валидации времени жизни
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                // параметры валидации подписи токена
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetIssuerSigningKey()
            };
        }

        // параметры авторизации
        public const string ADMIN_ROLE = "admin";

        private readonly AdminService adminService;

        public JwtService(AdminService adminService)
        {
            this.adminService = adminService;
        }

        // GenerateTokenAsync - генерация jwt-токена на основе api-ключа пользователля
        // вход: api-ключ пользователя
        // выход: строка jwt-токена
        // исключения: InvalidApiKeyException
        public async Task<string> GenerateTokenAsync(string apiKey)
        {
            try
            {
                // 1. получить пользователя по api-ключу
                Admin? admin = await adminService.GetAdminAsync(apiKey);
                // 2. подготовить данные пользователя (claims)
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, admin.Name),
                    new Claim(ClaimTypes.NameIdentifier, admin.Login),
                };
                if (admin.Role=="admin")
                {
                    // если пользователь VIP, то добавим ему роль
                    claims.Add(new Claim(ClaimTypes.Role, ADMIN_ROLE));
                }
                // 3. подготовить подпись токена
                SigningCredentials signing = new SigningCredentials(
                    GetIssuerSigningKey(),
                    SecurityAlgorithms.HmacSha256
                );
                // 4. собрать токен
                JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: JWT_ISSUER,
                    audience: JWT_AUDIENCE,
                    claims: claims,
                    signingCredentials: signing,
                    expires: DateTime.UtcNow.AddMinutes(JWT_LIFE_TIME_MINUTES)
                );
                // 5. вернуть токен в виде строки
                string jwtStr = new JwtSecurityTokenHandler().WriteToken(jwt);
                return jwtStr;
            }
            catch (NotFoundException)
            {
                // пользователя нет -> апи-ключ не правильный
                throw new InvalidApiKeyException();
            }
        }
        private const string ISSUER_SIGNING_KEY_SEED = "seedseedseedseedseedseedseedseed";

        private static SecurityKey GetIssuerSigningKey()
        {

            byte[] seedBytes = Encoding.UTF8.GetBytes(ISSUER_SIGNING_KEY_SEED);
            return new SymmetricSecurityKey(seedBytes);
        }
    }
}
