using API.Attributes;
using API.Services.Products;
using Common.Base;
using Common.DTO.ProductDTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : BaseAPIController
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public ResponseBase Home(string? name, int? categoryId, [Required] int page = 1)
        {
            ResponseBase result = _service.List(false, name, categoryId, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpGet("List")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Manager(string? name, int? categoryId, [Required] int page = 1)
        {
            ResponseBase result = _service.List(true, name, categoryId, page);
            Response.StatusCode = result.Code;
            return result;
        }

        [HttpPost]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{productId}")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Detail([Required] Guid productId)
        {
            ResponseBase response = _service.Detail(productId);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{productId}")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Update([Required] Guid productId, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Update(productId, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{productId}")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Delete([Required] Guid productId)
        {
            ResponseBase response = _service.Delete(productId);
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
