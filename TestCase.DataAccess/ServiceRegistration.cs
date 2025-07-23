using Microsoft.Extensions.DependencyInjection;
using TestCase.DataAccess.IRepository;
using TestCase.DataAccess.Repository;

namespace TestCase.DataAccess;

public static class ServiceRegistration
{
    public static void AddDataAccessServices(this IServiceCollection services)
    {
        
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<ISubscriptionCompanyRepository,SubscriptionCompanyRepository>();
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
     
    }
}