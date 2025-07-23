namespace TestCase.Contracts.RequestModels.SubscriptionCompany;

public class NewSubscription
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal MonthlyPrice { get; set; }
}