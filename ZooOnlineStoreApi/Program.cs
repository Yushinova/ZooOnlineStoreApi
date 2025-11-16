using ZooOnlineStoreApi.Api.DTOs;
using ZooOnlineStoreApi.Api.Middeleware;
using ZooOnlineStoreApi.Crypto;
using ZooOnlineStoreApi.Model.Addresses;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Interfaces;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.ProductImages;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;
using ZooOnlineStoreApi.Storage;
var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddAutoMapper(options => options.AddProfile<MappingProfiles>());
var app = builder.Build();

app.MapControllers();
app.UseMiddleware<ErrorMiddleware>();

app.Run();
