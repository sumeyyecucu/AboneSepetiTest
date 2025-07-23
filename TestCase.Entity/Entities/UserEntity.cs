using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestCase.Entity.Entities;

public class UserEntity:BaseEntity
{
    [Required]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Required]
    public string? PasswordHash { get; set; }

    public string? Role { get; set; } 

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; } = DateTime.UtcNow.AddDays(7);
}

