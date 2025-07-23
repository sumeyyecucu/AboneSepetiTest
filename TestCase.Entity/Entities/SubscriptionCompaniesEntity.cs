using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestCase.Entity.Entities;

public class SubscriptionCompaniesEntity:BaseEntity
{ 
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public decimal MonthlyPrice { get; set; }
    
}