using TestCase.Business.IServices;
using TestCase.Contracts.RequestModels.SubscriptionCompany;
using TestCase.DataAccess.IRepository;
using TestCase.Entity.Entities;

namespace TestCase.Business.Services.EntityServices;

public class SubscriptionCompanyService(ISubscriptionCompanyRepository subscriptionCompanyRepository)
    : ISubscriptionCompanyService
{
    private readonly ISubscriptionCompanyRepository _subscriptionCompanyRepository = subscriptionCompanyRepository;

    public async Task<SubscriptionCompaniesEntity> AddSubscriptionCompanyAsync(NewSubscription newSubscription)
    {
        var newSubscriptionCompany = new SubscriptionCompaniesEntity()
        {
            Name = newSubscription.Name,
            Description = newSubscription.Description,
            Category = newSubscription.Category,
            MonthlyPrice = newSubscription.MonthlyPrice
        };
         await _subscriptionCompanyRepository.AddAsync(newSubscriptionCompany);
         return newSubscriptionCompany;

    }

    public async Task<List<SubscriptionCompaniesEntity>> ListSubscriptionCompaniesAsync()
    {
        return await _subscriptionCompanyRepository.GetAllAsync();
    }
}