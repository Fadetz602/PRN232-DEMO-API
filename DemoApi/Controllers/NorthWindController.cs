using DemoApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApi.Controllers
{
    public class NorthWindController : Controller
    {
        public readonly NorthwindContext _con;
        public NorthWindController(NorthwindContext con)
        {
            _con = con;
        }
        [HttpGet("Q1")]
        public IActionResult Bai1()
        {
            var obj = _con.Employees
                .Select(x => new
                {
                    FullName = x.FirstName.ToLower() + " " + x.LastName.ToLower(),
                    TitleOfCourtesy = x.TitleOfCourtesy
                })
                .ToList();

            return Ok(obj);
        }

        [HttpGet("Bai2")]
        public IActionResult Bai2()
        {
            var obj = _con.Employees
                .Select(x => new
                {
                    FullName = x.LastName.ToUpper() +" " + x.FirstName.ToUpper()
                }).ToList();
            return Ok(obj);
        }

        [HttpGet("Bai3/{country}")]
        public IActionResult Bai3(string country)
        {
            var obj = _con.Employees
                .Where(x => x.Country.Contains(country)) // lọc theo quốc gia
                .Select(x => new
                {
                    x.LastName,
                    x.FirstName,
                    x.Title,
                    x.City,
                    x.Country
                })
                .ToList();

            return Ok(obj);
        }

        [HttpGet("Bai4")]
        public IActionResult Bai4()
        {
            var obj = _con.Customers
                .Where(c => c.Country == "UK") 
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    c.ContactName,
                    c.ContactTitle,
                    c.Country
                })
                .ToList();

            return Ok(obj);
        }

        [HttpGet("Bai5")]
        public IActionResult Bai5()
        {
            var obj = _con.Customers
                .Where(c => c.Country == "Mexico") 
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    c.Address,
                    c.City,
                    c.Country
                })
                .ToList();

            return Ok(obj);
        }

        [HttpGet("Bai6")]
        public IActionResult Bai6()
        {
            var obj = _con.Customers
                .Where(c => c.Country == "Sweden") 
                .Select(c => new
                {
                    c.CustomerId,
                    c.CompanyName,
                    c.Phone,
                    c.Address,
                    c.City,
                    c.Country
                })
                .ToList();

            return Ok(obj);
        }


        [HttpGet("Bai7")]
        public IActionResult Bai7()
        {
            var obj = _con.Products
                .Where(p => p.UnitsInStock >= 5 && p.UnitsInStock <= 10) // lọc tồn kho 5-10
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.UnitPrice,
                    p.UnitsInStock
                })
                .ToList();

            return Ok(obj);
        }

        [HttpGet("Bai8")]
        public IActionResult Bai8()
        {
            var obj = _con.Products
                .Where(p => p.UnitsOnOrder >= 60 && p.UnitsOnOrder <= 100) 
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.UnitPrice,
                    p.ReorderLevel,
                    p.UnitsOnOrder
                })
                .ToList();

            return Ok(obj);
        }


        [HttpGet("Bai9/{year}")]
        public IActionResult Bai9(int year)
        {
            var obj = _con.Orders.Where(x => x.OrderDate.Value.Year == year);
            var emp = _con.Employees.Select(x => new
            {
                EmployeeId = x.EmployeeId,
                LastName = x.LastName,
                FirstName = x.FirstName,
                Title = x.Title,
                Year = year,
                TotalOrder = obj.Where(year => year.EmployeeId == x.EmployeeId).Count()
            });
            return Ok(emp);
        }
        double CountById(dynamic obj, dynamic id)
        {
            int c = 0;
            foreach (var x in obj)
            {
                if (x.Id == id)
                    c++;
            }
            return c;
        }

        [HttpGet("Bai10")]
        public IActionResult Bai10()
        {
            int year = 1998; 
            var ordersInYear = _con.Orders.Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == year);

            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    e.City,
                    e.Country,
                    TotalOrders = ordersInYear.Count(o => o.EmployeeId == e.EmployeeId)
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai11")]
        public IActionResult Bai11()
        {
            DateTime startDate = new DateTime(1998, 1, 1);
            DateTime endDate = new DateTime(1998, 7, 31);

            var ordersInRange = _con.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value >= startDate && o.OrderDate.Value <= endDate);

            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    e.HireDate,
                    TotalOrders = ordersInRange.Count(o => o.EmployeeId == e.EmployeeId)
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai13")]
        public IActionResult Bai13()
        {
            DateTime startDate = new DateTime(1996, 8, 1);
            DateTime endDate = new DateTime(1996, 8, 5);

            var result = _con.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value >= startDate && o.OrderDate.Value <= endDate)
                .Select(o => new
                {
                    o.OrderId,
                    OrderDay = o.OrderDate.Value.Day,
                    OrderMonth = o.OrderDate.Value.Month,
                    OrderYear = o.OrderDate.Value.Year,
                    o.Freight,
                    Tax = o.Freight >= 100 ? 0.10 : 0.05,
                    FreightWithTax = o.Freight * (o.Freight >= 100 ? 1.10m : 1.05m)
        })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai14")]
        public IActionResult Bai14()
        {
            string[] titleArray = { "Mr.", "Ms.", "Mrs." };
            var employees = _con.Employees.ToList();
            var result = employees
                .Where(e => e.TitleOfCourtesy != null && titleArray.Contains(e.TitleOfCourtesy))
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    TitleOfCourtesy = e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? "Male" :
                          (e.TitleOfCourtesy == "Ms." || e.TitleOfCourtesy == "Mrs." ? "Female" : "Unknown")
                })
                .ToList();

            return Ok(result);
        }


        [HttpGet("Bai15")]
        public IActionResult Bai15()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    TitleOfCourtesy = e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? "M" : (e.TitleOfCourtesy == "Ms." || e.TitleOfCourtesy == "Mrs." ? "F" : "Unknown")
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai16")]
        public IActionResult Bai16()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    TitleOfCourtesy = e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? "Male" : (e.TitleOfCourtesy == "Ms." || e.TitleOfCourtesy == "Mrs." ? "Female" : "N/A")
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai17")]
        public IActionResult Bai17()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    TitleOfCourtesy = e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? 1 : (e.TitleOfCourtesy == "Ms." || e.TitleOfCourtesy == "Mrs." ? 0 : 2)
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai18")]
        public IActionResult Bai18()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    FullName = e.LastName + " " + e.FirstName,
                    TitleOfCourtesy = e.TitleOfCourtesy,
                    Sex = e.TitleOfCourtesy == "Mr." ? "M" : (e.TitleOfCourtesy == "Ms." || e.TitleOfCourtesy == "Mrs." ? "F" : "N/A")
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai21")]
        public IActionResult Bai21()
        {
            DateTime startDate = new DateTime(1996, 7, 1);
            DateTime endDate = new DateTime(1996, 7, 5);

            var result = _con.OrderDetails
                .Include(od => od.Order)     
                .Include(od => od.Product)    
                    .ThenInclude(p => p.Category) 
                .Where(od => od.Order.OrderDate.HasValue &&
                             od.Order.OrderDate.Value >= startDate &&
                             od.Order.OrderDate.Value <= endDate)
                .Select(od => new
                {
                    od.Product.Category.CategoryId,
                    od.Product.Category.CategoryName,
                    od.ProductId,
                    od.Product.ProductName,
                    Day = od.Order.OrderDate.Value.Day,
                    Month = od.Order.OrderDate.Value.Month,
                    Year = od.Order.OrderDate.Value.Year,
                    Revenue = od.Quantity * od.UnitPrice
                })
                .OrderBy(x => x.CategoryId)
                .ThenBy(x => x.ProductId)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai22")]
        public IActionResult Bai22()
        {
            var result = _con.Orders
                .Where(o => o.ShippedDate.HasValue && o.RequiredDate.HasValue && o.ShippedDate.Value > o.RequiredDate.Value)
                .Select(o => new
                {
                    o.EmployeeId,
                    o.Employee.LastName,
                    o.Employee.FirstName,
                    o.OrderId,
                    o.OrderDate,
                    o.RequiredDate,
                    o.ShippedDate
                })
                .ToList();

            return Ok(result);
        }


        [HttpGet("Bai23")]
        public IActionResult Bai23()
        {
            var employees = _con.Employees
                .Select(e => new { CompanyName = e.FirstName + " " + e.LastName, Phone = e.HomePhone });

            var customers = _con.Customers
                .Where(c => c.CompanyName.StartsWith("W"))
                .Select(c => new { c.CompanyName, c.Phone });

            var result = employees.Concat(customers).ToList();

            return Ok(result);
        }


        [HttpGet("Bai24")]
        public IActionResult Bai24()
        {
            var result = _con.Orders
                .Where(o => o.OrderId == 10643)
                .Select(o => new
                {
                    o.Customer.CustomerId,
                    o.Customer.CompanyName,
                    o.Customer.ContactName,
                    o.Customer.ContactTitle
                })
                .FirstOrDefault();

            return Ok(result);
        }

        [HttpGet("Bai25")]
        public IActionResult Bai25()
        {
            var result = _con.OrderDetails
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.ProductName,
                    TotalOrdered = g.Sum(x => x.Quantity)
                })
                .Where(x => x.TotalOrdered >= 1200)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai26")]
        public IActionResult Bai26()
        {
            var result = _con.OrderDetails
                .GroupBy(od => new { od.ProductId, od.Product.ProductName, od.Product.SupplierId, od.Product.CategoryId })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.ProductName,
                    g.Key.SupplierId,
                    g.Key.CategoryId,
                    TotalOrdered = g.Sum(x => x.Quantity)
                })
                .Where(x => x.TotalOrdered >= 1400)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai27")]
        public IActionResult Bai27()
        {
            var categoryCounts = _con.Products
                .GroupBy(p => new { p.CategoryId, p.Category.CategoryName })
                .Select(g => new
                {
                    g.Key.CategoryId,
                    g.Key.CategoryName,
                    TotalProducts = g.Count()
                });

            var maxCount = categoryCounts.Max(c => c.TotalProducts);

            var result = categoryCounts
                .Where(c => c.TotalProducts == maxCount)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai28")]
        public IActionResult Bai28()
        {
            var categoryCounts = _con.Products
                .GroupBy(p => new { p.CategoryId, p.Category.CategoryName })
                .Select(g => new
                {
                    g.Key.CategoryId,
                    g.Key.CategoryName,
                    TotalProducts = g.Count()
                });

            var minCount = categoryCounts.Min(c => c.TotalProducts);

            var result = categoryCounts
                .Where(c => c.TotalProducts == minCount)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai29")]
        public IActionResult Bai29()
        {
            var totalRecords = _con.Customers.Count() + _con.Employees.Count();

            return Ok(new { TotalRecords = totalRecords });
        }

        [HttpGet("Bai30")]
        public IActionResult Bai30()
        {
            var empOrders = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    e.Title,
                    TotalOrders = _con.Orders.Count(o => o.EmployeeId == e.EmployeeId)
                });

            var minOrders = empOrders.Min(e => e.TotalOrders);

            var result = empOrders
                .Where(e => e.TotalOrders == minOrders)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai31")]
        public IActionResult Bai31()
        {
            var empOrders = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    e.Title,
                    TotalOrders = _con.Orders.Count(o => o.EmployeeId == e.EmployeeId)
                });

            var maxOrders = empOrders.Max(e => e.TotalOrders);

            var result = empOrders
                .Where(e => e.TotalOrders == maxOrders)
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai32")]
        public IActionResult Bai32()
        {
            var result = _con.Products
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.SupplierId,
                    p.CategoryId,
                    p.UnitsInStock
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai33")]
        public IActionResult Bai33()
        {
            var result = _con.Products
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.SupplierId,
                    p.CategoryId,
                    p.UnitsInStock
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai34")]
        public IActionResult Bai34()
        {
            var maxUnitsOnOrder = _con.Products.Max(p => p.UnitsOnOrder);

            var result = _con.Products
                .Where(p => p.UnitsOnOrder == maxUnitsOnOrder)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.SupplierId,
                    p.CategoryId,
                    p.UnitsOnOrder
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai35")]
        public IActionResult Bai35()
        {
            var maxReorderLevel = _con.Products.Max(p => p.ReorderLevel);

            var result = _con.Products
                .Where(p => p.ReorderLevel == maxReorderLevel)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.SupplierId,
                    p.CategoryId,
                    p.ReorderLevel
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("Bai36")]
        public IActionResult Bai36()
        {
            var empDelayedOrders = _con.Employees
    .Select(e => new
    {
        e.EmployeeId,
        e.LastName,
        e.FirstName,
        DelayedOrders = _con.Orders.Count(o =>
            o.EmployeeId == e.EmployeeId &&
            o.ShippedDate.HasValue &&
            o.RequiredDate.HasValue &&
            o.ShippedDate.Value > o.RequiredDate.Value)
    })
    .ToList();

            var maxDelayed = empDelayedOrders.Max(e => e.DelayedOrders);

            var result = empDelayedOrders.Where(e => e.DelayedOrders == maxDelayed).ToList();


            return Ok(result);
        }
        [HttpGet("Bai37")]
        public IActionResult Bai37()
        {
            var empDelayedOrders = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.LastName,
                    e.FirstName,
                    DelayedOrders = _con.Orders.Count(o =>
                        o.EmployeeId == e.EmployeeId &&
                        o.ShippedDate.HasValue &&
                        o.RequiredDate.HasValue &&
                        o.ShippedDate.Value > o.RequiredDate.Value)
                })
                .Where(e => e.DelayedOrders > 0)
                .ToList(); 

            var minDelayed = empDelayedOrders.Min(e => e.DelayedOrders);

            var result = empDelayedOrders
                .Where(e => e.DelayedOrders == minDelayed)
                .ToList();

            return Ok(result);
        }



        [HttpGet("Bai38")]
        public IActionResult Bai38()
        {
            var productOrders = _con.OrderDetails
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.ProductName,
                    TotalOrdered = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(p => p.TotalOrdered)
                .Take(3)
                .ToList();

            return Ok(productOrders);
        }

        [HttpGet("Bai39")]
        public IActionResult Bai39()
        {
            var productOrders = _con.OrderDetails
                .GroupBy(od => new { od.ProductId, od.Product.ProductName })
                .Select(g => new
                {
                    g.Key.ProductId,
                    g.Key.ProductName,
                    TotalOrdered = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(p => p.TotalOrdered)
                .Take(5)
                .ToList();

            return Ok(productOrders);
        }

        [HttpGet("ProductsWithCustomers")]
        public IActionResult ProductsWithCustomers()
        {
            var result = _con.Products
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    Customers = _con.OrderDetails
                        .Where(od => od.ProductId == p.ProductId)
                        .Select(od => od.Order.Customer.CompanyName)
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("ProductDetailWithCustomersAndShippers/{id}")]
        public IActionResult ProductDetailWithCustomersAndShippers(int id)
        {
            var result = _con.Products
                .Where(p => p.ProductId == id)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    Customers = _con.OrderDetails
                        .Where(od => od.ProductId == p.ProductId)
                        .Select(od => od.Order.Customer.CompanyName)
                        .Distinct()
                        .ToList(),
                    Shippers = _con.OrderDetails
                        .Where(od => od.ProductId == p.ProductId)
                        .Select(od => od.Order.ShipViaNavigation.CompanyName)
                        .Distinct()
                        .ToList()
                })
                .FirstOrDefault();

            return Ok(result);
        }

        [HttpGet("ProductsWithEmployees")]
        public IActionResult ProductsWithEmployees()
        {
            var result = _con.Products
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    Employees = _con.OrderDetails
                        .Where(od => od.ProductId == p.ProductId)
                        .Select(od => od.Order.Employee.FirstName + " " + od.Order.Employee.LastName)
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("EmployeesWithProducts")]
        public IActionResult EmployeesWithProducts()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    Products = _con.Orders
                        .Where(o => o.EmployeeId == e.EmployeeId)
                        .SelectMany(o => o.OrderDetails.Select(od => od.Product.ProductName))
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("EmployeesWithShippers")]
        public IActionResult EmployeesWithShippers()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    Shippers = _con.Orders
                        .Where(o => o.EmployeeId == e.EmployeeId)
                        .SelectMany(o => o.OrderDetails.Select(od => od.Order.ShipViaNavigation.CompanyName))
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("EmployeesWithCategorySales")]
        public IActionResult EmployeesWithCategorySales()
        {
            var result = _con.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    Categories = _con.Orders
                        .Where(o => o.EmployeeId == e.EmployeeId)
                        .SelectMany(o => o.OrderDetails)
                        .GroupBy(od => new { od.Product.CategoryId, od.Product.Category.CategoryName })
                        .Select(g => new
                        {
                            g.Key.CategoryId,
                            g.Key.CategoryName,
                            TotalSales = g.Sum(x => x.Quantity * x.UnitPrice)
                        })
                        .ToList()
                })
                .ToList();

            return Ok(result);
        }


    }



}
