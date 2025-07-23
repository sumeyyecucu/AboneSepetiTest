using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TestCase.Entity.Entities;

namespace TestCase.DataAccess.Mongo;


public class MongoDbContext(IOptions<MongoDbSettings> settings, IMongoClient client)
{
    private readonly IMongoDatabase _database = client.GetDatabase(settings.Value.DatabaseName);

    public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("Users");
    public IMongoCollection<SubscriptionCompaniesEntity> Products => _database.GetCollection<SubscriptionCompaniesEntity>("Products");
}
