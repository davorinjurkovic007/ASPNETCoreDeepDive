using Globomantics.Domain.Models;

namespace Globomantics.Infrastructure.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    Cart CreateOrUpdate(Guid? cartId, Guid productId, int quantity = 1);
}
