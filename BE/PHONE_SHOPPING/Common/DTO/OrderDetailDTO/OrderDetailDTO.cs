using Common.DTO.OrderDTO;

namespace Common.DTO.OrderDetailDTO
{
    public class OrderDetailDTO : OrderListDTO
    {
        public OrderDetailDTO() 
        {
            DetailDTOs = new List<DetailDTO>();    
        }
        public List<DetailDTO> DetailDTOs { get; set; }
    }
}
