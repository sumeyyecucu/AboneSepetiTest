using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity.Data;
using TestCase.Business.IServices;

using TestCase.Contracts.RequestModels.Auth;
using TestCase.Contracts.ResponseModels;
using TestCase.Contracts.ResponseModels.Auth;
using TestCase.DataAccess.IRepository;
using TestCase.Entity.Entities;

namespace TestCase.Business.Services.EntityServices;

public class AuthService(IPasswordHasher passwordHasher,IRepository<UserEntity> userRepository,ITokenService tokenService) :IAuthService
{

    public async Task<List<UserEntity>> GetAllEntityAsync()
    {
        return await userRepository.GetAllAsync();
    }

    public async Task<AuthResponse> AddEntityAsync(Register register, string role)
    {
        var user = await userRepository.GetSingleAsync(x => x.PhoneNumber == register.PhoneNumber);
        if (user != null)
        {
            throw new Exception("User already exists");
        }

        if (register.Password != register.ConfirmPassword)
        {
            throw new Exception("Passwords do not match");
        }

        var refreshToken = tokenService.CreateRefreshToken();
        var expireTime = DateTime.Now.AddDays(14);

        user = new UserEntity()
        {
            PhoneNumber = register.PhoneNumber,
            PasswordHash = passwordHasher.Hash(register.Password),
            Role = role,
            RefreshToken = refreshToken,
            RefreshTokenExpireTime = expireTime,
        };
        await userRepository.AddAsync(user);
        var accessToken = tokenService.CreateAccessToken(user);
        var response = new AuthResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return response;

    }

    public async Task<AuthResponse> Login(Login login)
    {
        var user = await userRepository.GetSingleAsync(x => x.PhoneNumber == login.PhoneNumber);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        if (!passwordHasher.Verify(user.PasswordHash, login.Password))
        {
            throw new Exception("Passwords do not match");
        }
        var refreshToken = tokenService.CreateRefreshToken(); 
        var accessToken = tokenService.CreateAccessToken(user);
        var response = new AuthResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return response;

    }

    public async Task<AuthResponse> RefreshToken(string userId)
    {
        var user = await userRepository.GetSingleAsync(x => x.Id == userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.RefreshTokenExpireTime < DateTime.Now)
        {
            throw new Exception("Refresh token expired");
        }
        var refreshToken = tokenService.CreateRefreshToken();
        var expireTime = DateTime.Now.AddDays(14);
        var accessToken = tokenService.CreateAccessToken(user);
        var updatedUser = new UserEntity()
        {
            RefreshToken = refreshToken,
            RefreshTokenExpireTime = expireTime,
        };
        await userRepository.UpdateAsync(updatedUser);
        var response = new AuthResponse()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return response;

    }

    public async Task<bool> UpdatePassword(string userId,ChangePassword changePassword)
    {
        var user= await userRepository.GetSingleAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!passwordHasher.Verify(user.PasswordHash, changePassword.OldPassword))
        {
            throw new Exception("Passwords is not correct");
        }

        if (changePassword.NewPassword != changePassword.ConfirmNewPassword)
        {
            throw new Exception("Passwords do not match");
        }
        var newPassword = passwordHasher.Hash(changePassword.NewPassword);
        user.PasswordHash = newPassword;
        await userRepository.UpdateAsync(user);
        return true;
    }
    
}

