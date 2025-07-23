using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestCase.Business.Configuration;
using TestCase.Business.IServices;
using TestCase.Entity.Entities;

namespace TestCase.Business.Services;

public class TokenService(IOptions<JwtSettings> jwtOptions) : ITokenService
{

    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public string CreateAccessToken(UserEntity user)
    {
        if (string.IsNullOrEmpty(user.Role))
            throw new Exception("User role is not valid.");

        if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
            throw new Exception("JWT secret key is not configured.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public string CreateRefreshToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

}
