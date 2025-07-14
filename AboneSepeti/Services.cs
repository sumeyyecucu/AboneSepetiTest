using System.Reflection.Emit;
using AboneSepeti;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class Services
{
    private readonly IMongoCollection<UserModel> _user;
    private readonly TokenService _tokenService;

    public Services(IOptions<MongoDbSettings> settings, TokenService tokenService)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _user = database.GetCollection<UserModel>("Users");
        _tokenService = tokenService;
    }
    
    public async Task<AuthResponse> Register(Register newUser,string role) 
    {
        var existingUser = await _user.Find(u => u.PhoneNumber == newUser.PhoneNumber).FirstOrDefaultAsync();
        if (existingUser != null)
            throw new Exception("Bu numaraya sahip kullanıcı zaten kayıtlı.");

        if (newUser.Password != newUser.ConfirmPassword)
            throw new Exception("Şifrenizi doğru girmediniz");
        var refreshToken = _tokenService.GenerateRefreshToken();
        var expireTime = DateTime.UtcNow.AddDays(7);
        var user = new UserModel
        {
            PhoneNumber = newUser.PhoneNumber,
            PasswordHash = TokenService.Hash(newUser.Password),
            Role = role,
            RefreshToken = refreshToken,
            RefreshTokenExpireTime = expireTime
        };


        await _user.InsertOneAsync(user);
        var accessToken = _tokenService.GenerateAccessToken(user.Id.ToString(), user.Role); 
        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    public async Task<AuthResponse> LoginUser(Login loginRequest)
    {

        var user = await _user.Find(u => u.PhoneNumber == loginRequest.PhoneNumber).FirstOrDefaultAsync();
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı");

        var passwordMatch = TokenService.Verify(loginRequest.Password, user.PasswordHash);
        if (!passwordMatch)
            throw new Exception("Parola yanlış");


        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Role);
        var refreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);


        var update = Builders<UserModel>.Update
     .Set(u => u.RefreshToken, refreshToken)
     .Set(u => u.RefreshTokenExpireTime, DateTime.UtcNow.AddDays(7));



        await _user.UpdateOneAsync(u => u.Id == user.Id, update);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    public async Task<AuthResponse> RefreshToken(string userId)
    {
        var user = await _user.Find(u => u.Id == userId).FirstOrDefaultAsync();
        if (user == null)
            throw new Exception("Kullanıcı bulunamadı");
        if (user.RefreshTokenExpireTime < DateTime.UtcNow)
            throw new Exception("Refresh token geçersiz veya süresi dolmuş");

        var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.Role);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newExpireTime = DateTime.UtcNow.AddDays(7);

        var update = Builders<UserModel>.Update
            .Set(u => u.RefreshToken, newRefreshToken)
            .Set(u => u.RefreshTokenExpireTime, newExpireTime);

        await _user.UpdateOneAsync(u => u.Id == user.Id, update);

        return new AuthResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,

        };

    }
   

}


