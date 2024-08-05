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
    [Authorize]
    public class ProductController : BaseAPIController
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public ResponseBase List(string? name, int? categoryId, [Required] int page = 1)
        {
            ResponseBase response = _service.List(name, categoryId, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role(Roles.Admin)]
        public ResponseBase Create([Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{productId}")]
        [Role(Roles.Admin)]
        public ResponseBase Detail([Required] Guid productId)
        {
            ResponseBase response = _service.Detail(productId);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{productId}")]
        [Role(Roles.Admin)]
        public ResponseBase Update([Required] Guid productId, [Required] ProductCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Update(productId, DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpDelete("{productId}")]
        [Role(Roles.Admin)]
        public ResponseBase Delete([Required] Guid productId)
        {
            ResponseBase response = _service.Delete(productId);
            Response.StatusCode = response.Code;
            return response;
        }

    }
}
