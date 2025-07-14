using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AboneSepeti
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Password)]
         public string? PasswordHash { get; set; }
        public string? Role { get; set; } = "User";
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
      
    }
}
