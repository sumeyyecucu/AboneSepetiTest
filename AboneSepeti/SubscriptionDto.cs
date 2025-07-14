using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AboneSepeti
{
    public class SubscriptionDto
    {
    public string? Name { get; set; } 
    public string? Description { get; set; } 
    public string? Category { get; set; }
    public decimal MonthlyPrice { get; set; } 
    }
}