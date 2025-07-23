using MongoDB.Driver;
using TestCase.Contracts.RequestModels.Auth;
using TestCase.DataAccess.IRepository;
using TestCase.Entity.Entities;

namespace TestCase.DataAccess.Repository;

public class UserRepository(IMongoDatabase database) : Repository<UserEntity>(database), IUserRepository
{
    
}

