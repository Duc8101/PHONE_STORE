namespace Common.DTO.OrderDetailDTO
{
    public class DetailDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
