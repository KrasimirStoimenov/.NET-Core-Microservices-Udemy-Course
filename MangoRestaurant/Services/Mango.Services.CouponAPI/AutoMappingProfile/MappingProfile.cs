namespace Mango.Services.CouponAPI.AutoMappingProfile;

using AutoMapper;

using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dtos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Coupon, CouponDto>().ReverseMap();
    }
}
