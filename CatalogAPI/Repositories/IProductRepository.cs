using System;
using CatalogAPI.Models;

namespace CatalogAPI.Repositories;

public interface IProductRepository : IRepository<Product>
{
    IEnumerable<Product> GetProductsByCategory(int categoryId);
}
