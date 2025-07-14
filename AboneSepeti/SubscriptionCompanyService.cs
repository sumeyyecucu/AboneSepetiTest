using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AboneSepeti
{
    public class SubscriptionCompanyService
    {
        private readonly IMongoCollection<SubscriptionCompany> _companies;
        public SubscriptionCompanyService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _companies = database.GetCollection<SubscriptionCompany>("SubscriptionCompanies");
        }
        public async Task<List<SubscriptionCompany>> GetAllSubscriptionCompanies()
        {
            return await _companies.Find(_ => true).ToListAsync();
        }
        public async Task<SubscriptionCompany> AddSubscriptionCompany(SubscriptionDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var company = new SubscriptionCompany
            {
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                MonthlyPrice = dto.MonthlyPrice
            };
    
            await _companies.InsertOneAsync(company);
            return company;
           
        }
    }
}