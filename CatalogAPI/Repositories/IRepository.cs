using System;
using System.Linq.Expressions;

namespace CatalogAPI.Repositories;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T? Get(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(int id);
}
