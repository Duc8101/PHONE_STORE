namespace Common.DTO.CartDTO
{
    public class CartListDTO : CartCreateDTO
    {
        public string ProductName { get; set; } = null!;
        public string Image { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
