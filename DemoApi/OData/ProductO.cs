using System.Collections.Generic;

namespace DemoApi.Models
{
    public class ProductO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        public string? QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual Supplier? Supplier { get; set; }
        public ProductO(Product p)
        {
            ProductId = p.ProductId;
            ProductName = p.ProductName;
            SupplierId = p.SupplierId;
            CategoryId = p.CategoryId;
            QuantityPerUnit = p.QuantityPerUnit;
            UnitPrice = p.UnitPrice;
            UnitsInStock = p.UnitsInStock;
            UnitsOnOrder = p.UnitsOnOrder;
            ReorderLevel = p.ReorderLevel;
            Discontinued = p.Discontinued;
        }

    } //Automapper noi sau
}
