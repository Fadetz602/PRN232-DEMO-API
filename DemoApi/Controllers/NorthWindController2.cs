using DemoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NorthWindController2 : ControllerBase
    {

        public readonly NorthwindContext _con;
        public NorthWindController2(NorthwindContext con)
        {
            _con = con;
        }
        //Đếm số lượng khách hàng theo từng quốc gia
        [HttpGet("CountCustomersByCountry")]
        public IActionResult CountCustomersByCountry()
        {
            var result = _con.Customers
                .GroupBy(c => c.Country)
                .Select(g => new
                {
                    Country = g.Key,
                    CustomerCount = g.Count()
                })
                .ToList();

            return Ok(result);
        }
    }
}
