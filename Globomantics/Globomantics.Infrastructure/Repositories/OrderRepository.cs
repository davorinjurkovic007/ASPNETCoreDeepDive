using Globomantics.Domain.Models;
using Globomantics.Infrastructure.Data;

namespace Globomantics.Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order>
{
    public OrderRepository(GlobomanticsContext context) : base(context)
    {
    }
}
