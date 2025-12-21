using AutoMapper;
using ZooOnlineStoreApi.Api.DTOs.Requests;
using ZooOnlineStoreApi.Api.DTOs.Responses;
using ZooOnlineStoreApi.Model.Addresses;
using ZooOnlineStoreApi.Model.Admins;
using ZooOnlineStoreApi.Model.Categories;
using ZooOnlineStoreApi.Model.Feedbacks;
using ZooOnlineStoreApi.Model.OrderItems;
using ZooOnlineStoreApi.Model.Orders;
using ZooOnlineStoreApi.Model.Payments;
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
        }
    }
 
    
}
