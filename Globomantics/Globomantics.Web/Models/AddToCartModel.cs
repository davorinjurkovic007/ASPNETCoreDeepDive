namespace Globomantics.Web.Models;

public class AddToCartModel
{
    public Guid? CartId { get; set; }

    public ProductModel? Product { get; set; } 
}
