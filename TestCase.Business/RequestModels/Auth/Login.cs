using System.ComponentModel.DataAnnotations;

namespace TestCase.Contracts.RequestModels.Auth;

public class Login
{
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}