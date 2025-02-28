using Globomantics.Domain.Models;
using Globomantics.Infrastructure.Data;

namespace Globomantics.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>
{
    public ProductRepository(GlobomanticsContext context) : base(context)
    {
    }
}
