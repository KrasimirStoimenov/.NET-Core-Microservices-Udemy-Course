namespace Mango.Services.ProductAPI.AutoMappingProfile;

using AutoMapper;

using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<Product, ProductDto>().ReverseMap();
    }
}
