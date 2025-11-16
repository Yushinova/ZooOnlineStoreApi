using AutoMapper;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Addresses;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.PetTypes;
using ZooOnlineStoreApi.Model.ProductImages;
using ZooOnlineStoreApi.Model.Products;
using ZooOnlineStoreApi.Model.Users;

namespace ZooOnlineStoreApi.Api.DTOs
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PetType, PetTypeResponse>();
            CreateMap<PetTypeResponse, PetType>();
            CreateMap<PetType, PetTypeShortResponse>();
            CreateMap<PetTypeShortResponse, PetType>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryResponse, Category>();
            CreateMap<CategoryRequest, Category>();
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductImage, ProductImageResponse>();
            CreateMap<ProductImageRequest, ProductImage>();
            CreateMap<Feedback, FeedbackResponse>();
            CreateMap<User, UserResponse>();
            CreateMap<User, UserAuthResponse>();
            CreateMap<Address, AddressResponse>();
            CreateMap<AddressRequest, Address>();
        }
    }
 
    
}
