using DemoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NorthWind2Controller : ControllerBase
    {

        public readonly NorthwindContext _con;
        public NorthWind2Controller(NorthwindContext con)
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

        //Tính tổng số đơn hàng của từng nhân viên
        [HttpGet("TotalOrdersByEmployee")]
        public IActionResult TotalOrdersByEmployee()
        {
            var result = _con.Employees
                .GroupJoin(
                    _con.Orders,
                    emp => emp.EmployeeId,
                    ord => ord.EmployeeId,
                    (emp, orders) => new
                    {
                        emp.EmployeeId,
                        EmployeeName = emp.FirstName + " " + emp.LastName,
                        TotalOrders = orders.Count()
                    }
                )
                .ToList();
                //        var result = _con.Employees
                //.Select(emp => new
                //{
                //    emp.EmployeeId,
                //    EmployeeName = emp.FirstName + " " + emp.LastName,
                //    TotalOrders = _con.Orders.Count(o => o.EmployeeId == emp.EmployeeId)
                //})
                //.ToList();

            return Ok(result);
        }
        //Tính trung bình đơn giá của mỗi danh mục sản phẩm.
      [HttpGet("AveragePriceByCategory")]
        public IActionResult AveragePriceByCategory()
        {
            var result = _con.Categories
             .GroupJoin(
                 _con.Products,
                 c => c.CategoryId,
                 p => p.CategoryId,
                 (c, products) => new
                 {
                     c.CategoryId,
                     c.CategoryName,
                     AveragePrice = products.Any() ? products.Average(p => p.UnitPrice) : 0
                 })
             .ToList();


            return Ok(result);
        }

        //Tìm số lượng sản phẩm trong mỗi danh mục.
        [HttpGet("ProductCountByCategory")]
        public IActionResult ProductCountByCategory()
        {
                    var result = _con.Categories
             .Select(c => new
             {
                 c.CategoryId,
                 c.CategoryName,
                 ProductCount = _con.Products.Count(p => p.CategoryId == c.CategoryId)
             })
             .ToList();

            return Ok(result);
        }

        //Tính tổng số tiền bán được của mỗi sản phẩm.
        [HttpGet("ProductTotalRevenue")]
        public IActionResult ProductTotalRevenue()
        {
            var result = _con.OrderDetails
                
                .GroupBy(o => o.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    ProductName = _con.Products
                        .Where(p => p.ProductId == x.Key)
                        .Select(p => p.ProductName)
                        .FirstOrDefault(),
                    ProductTotalRevenue = x.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))

                })
                .OrderBy(o => o.ProductId).ToList();
            return Ok(result);
        }

        //Liệt kê 5 sản phẩm có đơn giá cao nhất.
        [HttpGet("Top5MaxPriceProduct")]
        public IActionResult Top5MaxPriceProduct()
        {
            var result = _con.Products
                .OrderByDescending(p => p.UnitPrice) 
                .Select(x => new
                {
                    x.ProductId,
                    x.ProductName,
                    x.UnitPrice
                })
                .Take(5) 
                .ToList();

            return Ok(result);
        }

        //Tìm các sản phẩm có đơn giá từ 50 đến 100.
        [HttpGet("ProductsPrice50To100")]
        public IActionResult ProductsPrice50To100()
        {
            var result = _con.Products
                .Where(p => p.UnitPrice >= 50 && p.UnitPrice <= 100)
                .ToList();

            return Ok(result);
        }

        //Tìm các khách hàng có tên bắt đầu bằng chữ “A”.
        [HttpGet("CustomersStartWithA")]
        public IActionResult CustomersStartWithA()
        {
            var result = _con.Customers
                .Where(c => c.ContactName.StartsWith("A"))
                .ToList();

            return Ok(result);
        }

        //Tìm đơn hàng có ngày đặt hàng trong năm 1997.
        [HttpGet("OrdersIn1997")]
        public IActionResult OrdersIn1997()
        {
            var result = _con.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == 1997)
                .ToList();

            return Ok(result);
        }

        //Tính doanh thu của từng đơn hàng.
        [HttpGet("OrderRevenue")]
        public IActionResult OrderRevenue()
        {
            var result = _con.Orders
                .Select(o => new
                {
                    o.OrderId,
                    Revenue = _con.OrderDetails
                        .Where(od => od.OrderId == o.OrderId)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                })
                .ToList();

            return Ok(result);
        }

        //Tính tuổi của từng nhân viên.
        [HttpGet("EmployeeAge")]
        public IActionResult EmployeeAge()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    EmployeeName = e.FirstName + " " + e.LastName,
                    Age = e.BirthDate.HasValue ?
                          DateTime.Now.Year - e.BirthDate.Value.Year -
                          (DateTime.Now.DayOfYear < e.BirthDate.Value.DayOfYear ? 1 : 0)
                          : 0
                })
                .ToList();

            return Ok(result);
        }

        //Hiển thị số năm làm việc của mỗi nhân viên.
        [HttpGet("EmployeeWorkingYears")]
        public IActionResult EmployeeWorkingYears()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    EmployeeName = e.FirstName + " " + e.LastName,
                    WorkingYears = e.HireDate.HasValue ?
                        DateTime.Now.Year - e.HireDate.Value.Year -
                        (DateTime.Now.DayOfYear < e.HireDate.Value.DayOfYear ? 1 : 0)
                        : 0
                })
                .ToList();

            return Ok(result);
        }

        //Tìm danh mục có nhiều hơn 10 sản phẩm.
        [HttpGet("CategoriesWithMoreThan10Products")]
        public IActionResult CategoriesWithMoreThan10Products()
        {
            var result = _con.Categories
                .Where(c => _con.Products.Count(p => p.CategoryId == c.CategoryId) > 10)
                .Select(c => new
                {
                    c.CategoryId,
                    c.CategoryName,
                    ProductCount = _con.Products.Count(p => p.CategoryId == c.CategoryId)
                })
                .ToList();

            return Ok(result);
        }

        //Tính tổng doanh thu của từng năm.
        [HttpGet("TotalRevenueByYear")]
        public IActionResult TotalRevenueByYear()
        {
            var result = _con.Orders
                .Where(o => o.OrderDate.HasValue)
                .GroupBy(o => o.OrderDate.Value.Year)
                .Select(g => new
                {
                    Year = g.Key,
                    TotalRevenue = g.Sum(o => _con.OrderDetails
                        .Where(od => od.OrderId == o.OrderId)
                        .Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount)))
                })
                .ToList();

            return Ok(result);
        }

        //Tìm sản phẩm có đơn giá lớn hơn đơn giá trung bình.
        [HttpGet("ProductsAboveAveragePrice")]
        public IActionResult ProductsAboveAveragePrice()
        {
            var avgPrice = _con.Products.Average(p => p.UnitPrice);
            var result = _con.Products
                .Where(p => p.UnitPrice > avgPrice)
                .ToList();

            return Ok(result);
        }

        //Tìm khách hàng có số lượng đơn hàng lớn hơn trung bình.
        [HttpGet("CustomersAboveAverageOrders")]
        public IActionResult CustomersAboveAverageOrders()
        {
            var avgOrders = _con.Customers.Average(c => _con.Orders.Count(o => o.CustomerId == c.CustomerId));
            var result = _con.Customers
                .Where(c => _con.Orders.Count(o => o.CustomerId == c.CustomerId) > avgOrders)
                .Select(c => new
                {
                    c.CustomerId,
                    c.ContactName,
                    TotalOrders = _con.Orders.Count(o => o.CustomerId == c.CustomerId)
                })
                .ToList();

            return Ok(result);
        }

        // 🟢 CREATE: Thêm sản phẩm mới
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest();

            _con.Products.Add(product);
            _con.SaveChanges();
            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        // 🔵 READ: Lấy tất cả sản phẩm
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _con.Products.ToList();
            return Ok(products);
        }

        // 🔵 READ: Lấy sản phẩm theo ID
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _con.Products.Find(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // 🟡 UPDATE: Cập nhật sản phẩm
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.ProductId)
                return BadRequest("ID không khớp");

            var product = _con.Products.Find(id);
            if (product == null)
                return NotFound();

            product.ProductName = updatedProduct.ProductName;
            product.UnitPrice = updatedProduct.UnitPrice;
            product.CategoryId = updatedProduct.CategoryId;

            _con.SaveChanges();
            return NoContent(); // HTTP 204
        }

        // 🔴 DELETE: Xóa sản phẩm
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _con.Products.Find(id);
            if (product == null)
                return NotFound();

            _con.Products.Remove(product);
            _con.SaveChanges();

            return NoContent();
        }
    }
}
