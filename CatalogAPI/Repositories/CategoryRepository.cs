using System;
using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context)
        : base(context) { }

    public IEnumerable<Category> GetCategoriesProducts()
    {
        return _context.Categories.Include(x => x.Products).AsNoTracking().ToList();
    }
}
