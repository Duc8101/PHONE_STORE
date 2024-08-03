using Common.Base;
using Common.DTO.OrderDTO;

namespace API.Services.Orders
{
    public interface IOrderService
    {
        Task<ResponseBase> Create(OrderCreateDTO DTO, Guid userId);
        ResponseBase List(Guid? userId, string? status, bool isAdmin, int page);
        ResponseBase Detail(Guid orderId, Guid? userId);
        Task<ResponseBase> Update(Guid orderId, OrderUpdateDTO DTO);
    }
}
