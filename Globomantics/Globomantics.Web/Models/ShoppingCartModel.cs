using Globomantics.Domain.Models;

namespace Globomantics.Web.Models;

public class ShoppingCartModel
{
    public Cart? Cart { get; set; }
    public bool IsCompact { get; set; }
}
