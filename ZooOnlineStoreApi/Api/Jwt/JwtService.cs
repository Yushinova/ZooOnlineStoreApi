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
        public const string ADMIN_ROLE = "admin";
        public const string USER_ROLE = "user";
        //параметры jwt-схемы и токена
        private const string JWT_ISSUER = "ZooOnlineStoreApi_issuer";
        private const string JWT_AUDIENCE = "ZooOnlineStoreApi_audience";
        private const int JWT_LIFE_TIME_MINUTES = 1440; // 24 часа

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
            // ⚡ ВАЖНО: Добавляем логику для работы с куками
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Пробуем получить токен из кук
                    if (context.Request.Cookies.TryGetValue("adminToken", out var adminToken))
                    {
                        context.Token = adminToken;
                        Console.WriteLine("JWT token taken from adminToken cookie");
                    }
                    // Также проверяем userToken для пользователей
                    else if (context.Request.Cookies.TryGetValue("userToken", out var userToken))
                    {
                        context.Token = userToken;
                        Console.WriteLine("JWT token taken from userToken cookie");
                    }
                    // Если нет в куках, оставляем стандартную логику из header (для Postman)
                    return Task.CompletedTask;
                },

                // Опционально: логирование ошибок аутентификации
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },

                // Опционально: логирование успешной аутентификации
                OnTokenValidated = context =>
                {
                    var userName = context.Principal.Identity.Name;
                    var roles = context.Principal.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);

                    Console.WriteLine($"JWT Token validated for user: {userName}, Roles: {string.Join(", ", roles)}");
                    return Task.CompletedTask;
                }
            };
        }

        private readonly AdminService adminService;
        private readonly UserService userService;

        public JwtService(AdminService adminService, UserService userService)
        {
            this.adminService = adminService;
            this.userService = userService;
        }

        // GenerateTokenAsync - генерация jwt-токена на основе api-ключа пользователля
        // вход: api-ключ пользователя
        // выход: строка jwt-токена
        // исключения: InvalidApiKeyException
        public async Task<string> GenerateAdminTokenAsync(string apiKey)
        {
            try
            {
                Admin? admin = await adminService.GetAdminAsync(apiKey);
                List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.NameIdentifier, admin.Login),
            new Claim("UserType", "Admin") // Добавляем тип пользователя
        };

                if (admin.Role == "admin")
                {
                    claims.Add(new Claim(ClaimTypes.Role, ADMIN_ROLE));
                }

                return GenerateToken(claims);
            }
            catch (NotFoundException)
            {
                throw new InvalidApiKeyException();
            }
        }

        public async Task<string> GenerateUserTokenAsync(string apiKey)
        {
            try
            {
                User? user = await userService.GetUserAsync(apiKey);
                List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Phone),
            new Claim(ClaimTypes.Role, USER_ROLE),
            new Claim("UserType", "User") // Добавляем тип пользователя
        };

                return GenerateToken(claims);
            }
            catch (NotFoundException)
            {
                throw new InvalidApiKeyException();
            }
        }
        private string GenerateToken(List<Claim> claims)
        {
            SigningCredentials signing = new SigningCredentials(
                GetIssuerSigningKey(),
                SecurityAlgorithms.HmacSha256
            );

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: JWT_ISSUER,
                audience: JWT_AUDIENCE,
                claims: claims,
                signingCredentials: signing,
                expires: DateTime.UtcNow.AddMinutes(JWT_LIFE_TIME_MINUTES)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        private const string ISSUER_SIGNING_KEY_SEED = "seedseedseedseedseedseedseedseed";

        private static SecurityKey GetIssuerSigningKey()
        {

            byte[] seedBytes = Encoding.UTF8.GetBytes(ISSUER_SIGNING_KEY_SEED);
            return new SymmetricSecurityKey(seedBytes);
        }
    }
}
