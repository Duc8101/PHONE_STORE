namespace Common.DTO.OrderDTO
{
    public class OrderListDTO
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Note { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
