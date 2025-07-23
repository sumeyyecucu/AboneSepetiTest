using MongoDB.Driver;
using TestCase.DataAccess.IRepository;
using TestCase.Entity.Entities;

namespace TestCase.DataAccess.Repository;

public class SubscriptionCompanyRepository(IMongoDatabase database)
    : Repository<SubscriptionCompaniesEntity>(database), ISubscriptionCompanyRepository
{
  
}
