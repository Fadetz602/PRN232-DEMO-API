using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApi.Models;
using Microsoft.AspNetCore.OData.Query;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public AutoController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Auto
        [EnableQuery]
        [HttpGet]
        public IQueryable<ProductDTO> GetProducts()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    UnitsInStock = p.UnitsInStock,
                    Category = p.Category == null ? null : new CategoryDTO
                    {
                        CategoryId = p.Category.CategoryId,
                        CategoryName = p.Category.CategoryName
                    },
                    Supplier = p.Supplier == null ? null : new SupplierDTO
                    {
                        SupplierId = p.Supplier.SupplierId,
                        CompanyName = p.Supplier.CompanyName
                    }
                });
        }

        // GET: api/Auto/5
        [EnableQuery]
        [HttpGet("{id}")]
        public IQueryable<ProductO> GetProduct(int id)
        {
            return _context.Products
                .Where(p => p.ProductId == id)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Select(p => new ProductO(p));
        }

        // POST: api/Auto
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        // PUT: api/Auto/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId)
                return BadRequest();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.ProductId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Auto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
