using System.Linq.Expressions;

using TestCase.Contracts.RequestModels.Auth;
using TestCase.Contracts.ResponseModels;
using TestCase.Contracts.ResponseModels.Auth;
using TestCase.Entity.Entities;

namespace TestCase.Business.IServices;

public interface IAuthService 
{
    Task<List<UserEntity>> GetAllEntityAsync();
    Task<bool> UpdatePassword(string userId, ChangePassword changePassword);
    Task<AuthResponse> AddEntityAsync(Register entity,string role);
    Task<AuthResponse> Login(Login login);
    Task<AuthResponse> RefreshToken(string userId);
    

}