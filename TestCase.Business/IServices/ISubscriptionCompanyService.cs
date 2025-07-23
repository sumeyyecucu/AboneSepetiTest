using TestCase.Contracts.RequestModels.SubscriptionCompany;
using TestCase.Entity.Entities;

namespace TestCase.Business.IServices;

public interface  ISubscriptionCompanyService 
{
    Task<SubscriptionCompaniesEntity> AddSubscriptionCompanyAsync(NewSubscription newSubscription);
    Task<List<SubscriptionCompaniesEntity>> ListSubscriptionCompaniesAsync();
}