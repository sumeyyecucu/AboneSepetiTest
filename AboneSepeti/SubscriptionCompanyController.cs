using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AboneSepeti
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionCompanyController : ControllerBase
    {
        private readonly SubscriptionCompanyService _service;
        public SubscriptionCompanyController(SubscriptionCompanyService service)
        {
            _service = service;
        }
        [HttpGet("companies")] //Admin ve User erişebilir
        public async Task<IActionResult> GetSubscriptionCompanies()
        {
           var companies= await _service.GetAllSubscriptionCompanies();
            return Ok(companies);
        }
        [HttpPost("subscribe")] //Deneme amaçlı eklendi
        public async Task<IActionResult> AddSubscriptionCompany([FromBody] SubscriptionDto dto)
        {
            var newCompany = await _service.AddSubscriptionCompany(dto);
            return Ok(newCompany);
        }
    }
}