using CatalogAPI.Context;
using CatalogAPI.Models;

namespace CatalogAPI.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context)
        : base(context) { }

    public IEnumerable<Product> GetProductsByCategory(int categoryId)
    {
        return GetAll().Where(w => w.CategoryId == categoryId);
    }
}
