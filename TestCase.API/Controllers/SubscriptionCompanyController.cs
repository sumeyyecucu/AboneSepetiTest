using Microsoft.AspNetCore.Mvc;
using TestCase.Business.IServices;
using TestCase.Contracts.RequestModels.SubscriptionCompany;
using TestCase.DataAccess.IRepository;

namespace TestCase.API.Controllers;

[ApiController]
[Route("api/subscription")]
public class SubscriptionCompanyController(ISubscriptionCompanyService subscriptionCompanyService)
    : ControllerBase
{
    private readonly ISubscriptionCompanyService _subscriptionCompanyService = subscriptionCompanyService;

    [HttpPost]
    [Route("addCompany")]
    public async Task<IActionResult> AddNewCompany(NewSubscription subscriptionDto)
    {
       var company= await _subscriptionCompanyService.AddSubscriptionCompanyAsync(subscriptionDto);
       return Ok(company);
    }

    [HttpGet]
    [Route("getAllCompany")]
    public async Task<IActionResult> GetAllCompany()
    {
        var companies = await _subscriptionCompanyService.ListSubscriptionCompaniesAsync();
        return Ok(companies);
    }
}