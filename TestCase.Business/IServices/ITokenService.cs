using TestCase.Entity.Entities;

namespace TestCase.Business.IServices;

public interface ITokenService
{
    public string CreateAccessToken(UserEntity user);
    public string CreateRefreshToken();
}