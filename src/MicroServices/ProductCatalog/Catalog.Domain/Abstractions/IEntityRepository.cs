using Catalog.Domain.Entities;

namespace Catalog.Domain.Abstractions;

public interface IEntityRepository<T>
    where T : EntityBase
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task RemoveAsync(int id);
}
