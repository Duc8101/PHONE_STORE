namespace Common.DTO.ProductDTO
{
    public class ProductListDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
