namespace DemoApi.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public CategoryDTO? Category { get; set; }

        public SupplierDTO? Supplier { get; set; }
    }
}
