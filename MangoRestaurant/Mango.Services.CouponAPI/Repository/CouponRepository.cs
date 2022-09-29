namespace Mango.Services.CouponAPI.Repository;

using AutoMapper;

using Mango.Services.CouponAPI.DbContext;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public CouponRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }
}
