using DemoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly NorthwindContext _con;
        public ProductController(NorthwindContext con)
        {
            _con = con;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var obj = _con.Products.
                Select(x=> new ProductO(x)).
                ToList();
            return Ok(obj);
        }
        [HttpGet("Product/{id}")]
        public IActionResult Getabc(int id)
        {
            //var obj = _con.Products.Find(id);
            var obj = _con.Products.FirstOrDefault(x=>x.ProductId == id);
            return Ok(obj);
        }
    }
}
