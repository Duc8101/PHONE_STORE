using AutoMapper;
using Common.DTO.CartDTO;
using Common.DTO.CategoryDTO;
using Common.DTO.OrderDetailDTO;
using Common.DTO.OrderDTO;
using Common.DTO.ProductDTO;
using Common.DTO.UserDTO;
using Common.Entity;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductListDTO>()
                .ForMember(d => d.CategoryName, m => m.MapFrom(source => source.Category.CategoryName));
            CreateMap<Category, CategoryListDTO>();
            CreateMap<User, UserDetailDTO>()
                .ForMember(d => d.RoleName, m => m.MapFrom(source => source.Role.RoleName));
            CreateMap<Cart, CartListDTO>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.Product.ProductName))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Product.Image))
                .ForMember(d => d.Price, m => m.MapFrom(source => source.Product.Price));
            CreateMap<UserCreateDTO, User>()
                .ForMember(d => d.Email, m => m.MapFrom(source => source.Email.Trim()));
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<CartListDTO, OrderDetail>();
            CreateMap<Order, OrderListDTO>()
                .ForMember(d => d.Username, m => m.MapFrom(source => source.User.Username))
                .ForMember(d => d.OrderDate, m => m.MapFrom(source => source.CreatedAt));
            CreateMap<OrderDetail, DetailDTO>();
            CreateMap<ProductCreateUpdateDTO, Product>()
                .ForMember(d => d.ProductName, m => m.MapFrom(source => source.ProductName.Trim()))
                .ForMember(d => d.Image, m => m.MapFrom(source => source.Image.Trim()));
            CreateMap<Order, OrderDetailDTO>()
                .ForMember(d => d.Username, m => m.MapFrom(source => source.User.Username))
                .ForMember(d => d.OrderDate, m => m.MapFrom(source => source.CreatedAt));
        }
    }
}
