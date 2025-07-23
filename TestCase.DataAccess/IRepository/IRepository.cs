using System.Linq.Expressions;
using TestCase.Entity.Entities;

namespace TestCase.DataAccess.IRepository;

public interface IRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetSingleAsync(Expression<Func<T, bool>> method);
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
   
}