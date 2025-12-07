using Microsoft.AspNetCore.Authentication.JwtBearer;
using ZooOnlineStoreApi.Api.DTOs;
using ZooOnlineStoreApi.Api.Jwt;
using ZooOnlineStoreApi.Api.Middeleware;
using ZooOnlineStoreApi.Crypto;
using ZooOnlineStoreApi.Model.Addresses;
using ZooOnlineStoreApi.Model.Admins;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.ProductImages;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;
using ZooOnlineStoreApi.Storage;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:3001", "https://localhost:3001")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddControllers();
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
builder.Services.AddAutoMapper(options => options.AddProfile<MappingProfiles>());

// сервисы аутентификации и авторизации
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtService.ConfigureJwtOptions);


builder.Services.AddAuthorization();
builder.Services.AddTransient<JwtService>();
var app = builder.Build();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
app.UseMiddleware<ErrorMiddleware>();
// добавить middleware аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.Run();
