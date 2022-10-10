namespace Mango.Services.ShoppingCartAPI.Repository;

using System.Threading.Tasks;

using AutoMapper;

using Mango.Services.ShoppingCartAPI.DbContext;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;

using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;

    public CartRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    public async Task<CartDto> GetCartByUserId(string userId)
    {
        Cart cart = new Cart
        {
            CartHeader = await this.dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId)
        };

        cart.CartDetails = this.dbContext.CartDetails
            .Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId)
            .Include(x => x.Product);

        return this.mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
    {
        Cart cart = this.mapper.Map<Cart>(cartDto);

        Product prodInDb = await this.dbContext.Products
            .FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

        if (prodInDb == null)
        {
            this.dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
            await this.dbContext.SaveChangesAsync();
        }

        CartHeader cartHeaderFromDb = await this.dbContext.CartHeaders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == cart.CartHeader.UserId);

        if (cartHeaderFromDb == null)
        {
            this.dbContext.CartHeaders.Add(cart.CartHeader);
            await this.dbContext.SaveChangesAsync();
            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
            cart.CartDetails.FirstOrDefault().Product = null;
            this.dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
            await this.dbContext.SaveChangesAsync();
        }
        else
        {
            CartDetails cartDetailsFromDb = await this.dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    x => x.ProductId == cart.CartDetails.FirstOrDefault().ProductId
                    && x.CartHeaderId == cartHeaderFromDb.CartHeaderId);

            if (cartDetailsFromDb == null)
            {
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                this.dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await this.dbContext.SaveChangesAsync();
            }
            else
            {
                cart.CartDetails.FirstOrDefault().Product = null;
                cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                this.dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                await this.dbContext.SaveChangesAsync();
            }
        }

        return this.mapper.Map<CartDto>(cart);
    }

    public async Task<bool> RemoveFromCart(int cartDetailsId)
    {
        try
        {
            CartDetails cartDetails = await this.dbContext.CartDetails
                .FirstOrDefaultAsync(x => x.CartDetailsId == cartDetailsId);

            int totalCountOfCartItems = this.dbContext.CartDetails
                .Where(x => x.CartHeaderId == cartDetails.CartHeaderId)
                .Count();

            this.dbContext.CartDetails.Remove(cartDetails);
            if (totalCountOfCartItems == 1)
            {
                CartHeader cartHeaderToRemove = await this.dbContext.CartHeaders
                    .FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);

                this.dbContext.CartHeaders.Remove(cartHeaderToRemove);
            }

            await this.dbContext.SaveChangesAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ClearCart(string userId)
    {
        CartHeader cartHeaderFromDb = await this.dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);

        if (cartHeaderFromDb != null)
        {
            this.dbContext.CartDetails
                .RemoveRange(this.dbContext.CartDetails.Where(x => x.CartHeaderId == cartHeaderFromDb.CartHeaderId));
            this.dbContext.CartHeaders.Remove(cartHeaderFromDb);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        var cartFromDb = await this.dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
        cartFromDb.CouponCode = couponCode;

        this.dbContext.CartHeaders.Update(cartFromDb);
        await this.dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        var cartFromDb = await this.dbContext.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId);
        cartFromDb.CouponCode = "";

        this.dbContext.CartHeaders.Update(cartFromDb);
        await this.dbContext.SaveChangesAsync();

        return true;
    }
}
