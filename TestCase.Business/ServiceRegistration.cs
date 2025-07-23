using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TestCase.Business.Configuration;
using TestCase.Business.IServices;
using TestCase.Business.Services;
using TestCase.Business.Services.EntityServices;

namespace TestCase.Business;

public static class ServiceRegistration
{
    
        public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPasswordHasher, PasswordHasher>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ISubscriptionCompanyService, SubscriptionCompanyService>();
            
            
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            if (string.IsNullOrEmpty(jwtSettings?.SecretKey))
                throw new Exception("JWT Secret cannot be null or empty.");
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings")); 

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"])),
                        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                            expires != null && expires > DateTime.UtcNow
                    };
                });
        }
    }
