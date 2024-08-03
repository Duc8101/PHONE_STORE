using API.Attributes;
using API.Services.Carts;
using Common.Base;
using Common.DTO.CartDTO;
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
    [Role(Roles.Customer)]
    [Authorize]
    public class CartController : BaseAPIController
    {
        private readonly ICartService _service;
        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public ResponseBase List()
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.List(Guid.Parse(userId));
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        public ResponseBase Create([Required] CartCreateDTO DTO)
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Create(DTO, Guid.Parse(userId));
            }
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete]
        public ResponseBase Delete([Required] Guid productId)
        {
            string? userId = getUserId();
            ResponseBase response;
            if (userId == null)
            {
                response = new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            else
            {
                response = _service.Delete(productId, Guid.Parse(userId));
            }
            Response.StatusCode = response.Code;
            return response;
        }
    }
}
