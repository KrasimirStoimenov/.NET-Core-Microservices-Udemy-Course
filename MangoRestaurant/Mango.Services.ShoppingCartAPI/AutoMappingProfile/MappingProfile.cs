namespace Mango.Services.ShoppingCartAPI.AutoMappingProfile;

using AutoMapper;

using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
        CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        CreateMap<Cart, CartDto>().ReverseMap();
    }
}
