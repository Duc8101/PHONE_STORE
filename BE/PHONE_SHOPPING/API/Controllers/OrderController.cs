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
using System.Security.Claims;

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
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = await _service.Create(DTO, Guid.Parse(userId));
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet]
        public ResponseBase List(string? status, [Required] int page = 1)
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user id", (int)HttpStatusCode.NotFound);
            }
            else
            {
                bool admin = isAdmin();
                response = _service.List(admin ? null : Guid.Parse(userId), status, admin, page);
                Response.StatusCode = response.Code;
            }
            return response;
        }

        [HttpGet("{orderId}")]
        public ResponseBase Detail([Required] Guid orderId)
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else if (isAdmin())
            {
                response = _service.Detail(orderId, null);
            }
            else
            {
                response = _service.Detail(orderId, Guid.Parse(userId));
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{orderId}")]
        [Role(Roles.Admin)]
        public async Task<ResponseBase> Update([Required] Guid orderId, [Required] OrderUpdateDTO DTO)
        {
            ResponseBase response = await _service.Update(orderId, DTO);
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
