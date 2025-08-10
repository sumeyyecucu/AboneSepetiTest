using System.ComponentModel.DataAnnotations;



namespace TestCase.Contracts.RequestModels.Auth;

public class Register
{
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}