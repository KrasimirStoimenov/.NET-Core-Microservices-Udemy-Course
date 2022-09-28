namespace Mango.Web.Models.CartModels;

public class CartModel
{
    public CartHeaderModel CartHeader { get; set; }

    public IEnumerable<CartDetailsModel> CartDetails { get; set; }
}
