using AutoMapper;
using ZooOnlineStoreApi.Models;
using ZooOnlineStoreApi.Services.DTOs.Requests;
using ZooOnlineStoreApi.Services.DTOs.Responses;

namespace ZooOnlineStoreApi.Services.DTOs
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PetType, PetTypeResponse>();
            CreateMap<PetTypeResponse, PetType>();
            CreateMap<PetType, PetTypeShortResponse>();
            CreateMap<PetTypeShortResponse, PetType>();
            CreateMap<PetTypeUpdate, PetType>();
            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryResponse, Category>();
            CreateMap<CategoryRequest, Category>();
            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductImage, ProductImageResponse>();
            CreateMap<ProductImageRequest, ProductImage>();
            CreateMap<Feedback, FeedbackResponse>();
            CreateMap<FeedbackRequest, Feedback>();
            CreateMap<User, UserResponse>();
            CreateMap<User, UserAuthResponse>();
            CreateMap<User, UserOrderResponse>();
            CreateMap<User, UserFeedbackResponse>();
            CreateMap<UserRequest, User>();
            CreateMap<Address, AddressResponse>();
            CreateMap<AddressRequest, Address>();
            CreateMap<OrderItem, OrderItemResponse>();
            CreateMap<OrderItemRequest, OrderItem>();
            CreateMap<Order, OrderResponse>();
            CreateMap<Order, OrderPaymentResponse>();
            CreateMap<OrderRequest, Order>();
            CreateMap<AdminRequest, Admin>();
            CreateMap<Admin, AdminResponse>();
            CreateMap<PaymentRequest, Payment>();
            CreateMap<Payment, PaymentResponse>();
            CreateMap<PaymentPaged, PaymentPagedResponse>();
            CreateMap<PaymentRequestParams, PaymentFilter>();
        }
    }
 
    
}
