namespace TestCase.Contracts.ResponseModels.Auth;

public class AuthResponse 
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}