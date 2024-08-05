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
            if (userId == null)
            {
                return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            return _service.List(Guid.Parse(userId));
        }

        [HttpPost]
        public ResponseBase Create([Required] CartCreateDTO DTO)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            return _service.Create(DTO, Guid.Parse(userId));
        }

        [HttpDelete]
        public ResponseBase Delete([Required] Guid productId)
        {
            string? userId = getUserId();
            if (userId == null)
            {
                return new ResponseBase("Not found user", (int)HttpStatusCode.NotFound);
            }
            return _service.Delete(productId, Guid.Parse(userId));
        }
    }
}
