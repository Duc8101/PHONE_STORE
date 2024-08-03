namespace Common.DTO.ProductDTO
{
    public class ProductCreateUpdateDTO
    {
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
