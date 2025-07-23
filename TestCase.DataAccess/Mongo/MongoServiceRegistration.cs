using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TestCase.DataAccess.Mongo;

   public static class MongoServiceRegistration
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(
                configuration.GetSection("MongoDbSettings"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = configuration
                    .GetSection("MongoDbSettings")
                    .Get<MongoDbSettings>();

                return new MongoClient(settings.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            services.AddScoped<MongoDbContext>();

            return services;
        }
    }
