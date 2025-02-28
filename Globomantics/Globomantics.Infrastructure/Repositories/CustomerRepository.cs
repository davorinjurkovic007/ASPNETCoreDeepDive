using Globomantics.Domain.Models;
using Globomantics.Infrastructure.Data;

namespace Globomantics.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>
{
    public CustomerRepository(GlobomanticsContext context) : base(context)
    {
    }
}
