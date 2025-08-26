using System.Linq.Expressions;
using CatalogAPI.Context;

namespace CatalogAPI.Repositories;

public class Repository<T> : IRepository<T>
    where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public T Create(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Set<T>().Add(entity);
        _context.SaveChanges();

        return entity;
    }

    public T Delete(int id)
    {
        var entity = _context.Find<T>(id);

        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Set<T>().Remove(entity);
        _context.SaveChanges();

        return entity;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _context.Set<T>().Update(entity);
        _context.SaveChanges();

        return entity;
    }
}
