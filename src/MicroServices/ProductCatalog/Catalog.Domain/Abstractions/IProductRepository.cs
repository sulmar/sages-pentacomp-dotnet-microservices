﻿using Catalog.Domain.Entities;

namespace Catalog.Domain.Abstractions;

public interface IProductRepository : IEntityRepository<Product>
{
    Task<IEnumerable<Product>> GetByColorAsync(string color);
}
