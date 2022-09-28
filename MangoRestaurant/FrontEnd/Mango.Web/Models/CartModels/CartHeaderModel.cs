namespace Mango.Web.Models.CartModels;

public class CartHeaderModel
{
    public int CartHeaderId { get; set; }

    public string UserId { get; set; }

    public string CouponCode { get; set; }

    public double OrderTotal { get; set; }
}
