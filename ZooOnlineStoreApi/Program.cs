using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Api.Middeleware;
using ZooOnlineStoreApi.Crypto;
using ZooOnlineStoreApi.Services;
using ZooOnlineStoreApi.Services.DTOs;
using ZooOnlineStoreApi.Services.Interfaces;
using ZooOnlineStoreApi.Storage;
using Microsoft.OpenApi.Models;
using System.Reflection;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                               "https://localhost:3000",
                               "http://localhost:3001",
                               "https://localhost:3001",
                               "http://zoo-admin.localhost:3000",
                               "http://zoo-customer.localhost:3001")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ZooOnlineStore API",
        Version = "v1",
        Description = "Zoo Online Store API"
    });
    // XML документация
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Настройка JWT для Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = @"
                    📋 Step-by-step:
                    1️ Get token: POST /api/SwaggerTest/admin-token (test)
                       OR POST /api/SwaggerTest/user-token (test)
                    2️ Copy 'Token' value from response
                    3️ Click 🔒 Authorize button above
                    4️ Enter: Bearer [your_token]
                    5️ Click Authorize → Close
                    6️ Test any protected endpoint
                    ",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddTransient(opts => EncoderFactory.CreateEncoderFactory());
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<PetTypeService>();
builder.Services.AddTransient<IPetTypeRepository, PetTypeRepository>();
builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductImageRepository, ProductImageRepository>();
builder.Services.AddTransient<ProductImageService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<AddressService>();
builder.Services.AddTransient<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddTransient<OrderItemService>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddTransient<FeedbackService>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<AdminService>();
builder.Services.AddTransient<JwtService>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
builder.Services.AddTransient<PaymentService>();
builder.Services.AddAutoMapper(options => options.AddProfile<MappingProfiles>());

// сервисы аутентификации и авторизации
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtService.ConfigureJwtOptions);


builder.Services.AddAuthorization();
builder.Services.AddTransient<JwtService>();
var app = builder.Build();
app.UseRouting();

app.UseMiddleware<ErrorMiddleware>();
app.UseCors(MyAllowSpecificOrigins);

// добавить middleware аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ZooOnlineStore API v1");
    options.RoutePrefix = "docs";
});
app.MapControllers();
app.Run();
