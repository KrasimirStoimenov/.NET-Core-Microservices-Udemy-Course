namespace Mango.Web.Models.CartModels;

public class CartDetailsModel
{
    public int CartDetailsId { get; set; }

    public int Count { get; set; }


    public int CartHeaderId { get; set; }
    public virtual CartHeaderModel CartHeader { get; set; }

    public int ProductId { get; set; }
    public virtual ProductModel Product { get; set; }
}
