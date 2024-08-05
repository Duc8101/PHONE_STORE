using API.Attributes;
using API.Services.Orders;
using Common.Base;
using Common.DTO.OrderDTO;
using Common.Entity;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OrderController : BaseAPIController
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost]
        [Role(Roles.Customer)]
        public async Task<ResponseBase> Create([Required] OrderCreateDTO DTO)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            return await _service.Create(DTO, Guid.Parse(userId));
        }

        [HttpGet]
        public ResponseBase List(string? status, [Required] int page = 1)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
            }
            bool admin = isAdmin();
            return _service.List(admin ? null : Guid.Parse(userId), status, admin, page);
        }

        [HttpGet("{orderId}")]
        public ResponseBase Detail([Required] Guid orderId)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }

            if (isAdmin())
            {
                return _service.Detail(orderId, null);
            }

            return _service.Detail(orderId, Guid.Parse(userId));
        }

        [HttpPut("{orderId}")]
        [Role(Roles.Admin)]
        public async Task<ResponseBase> Update([Required] Guid orderId, [Required] OrderUpdateDTO DTO)
        {
            return await _service.Update(orderId, DTO);
        }
    }
}
