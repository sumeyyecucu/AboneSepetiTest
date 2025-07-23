using System.Linq.Expressions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using TestCase.DataAccess.IRepository;
using TestCase.Entity.Entities;

namespace TestCase.DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public Repository(IMongoDatabase database)
    {
        var collectionName = typeof(T).Name + "s"; 
        _collection = database.GetCollection<T>(collectionName);
    }
    public async Task<List<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<List<T>> GetWhere(Expression<Func<T, bool>> method)
    {
        return await _collection.Find(method).ToListAsync();
    }

    public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> method)
    {
        return await _collection.Find(method).FirstOrDefaultAsync();
    }


    public async Task<T?> GetByIdAsync(string id)
    {
        return await _collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        return true;
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        return true;
    }

}