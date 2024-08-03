using API.Attributes;
using API.Services.Categories;
using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CategoryController : BaseAPIController
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("All")]
        public ResponseBase List()
        {
            ResponseBase response = _service.ListAll();
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("Paged")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase List(string? name, [Required] int page = 1)
        {
            ResponseBase response = _service.ListPaged(name, page);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPost]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Create([Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Create(DTO);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpGet("{categoryId}")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Detail([Required] int categoryId)
        {
            ResponseBase response = _service.Detail(categoryId);
            Response.StatusCode = response.Code;
            return response;
        }

        [HttpPut("{categoryId}")]
        [Role(Roles.Admin)]
        [Authorize]
        public ResponseBase Update([Required] int categoryId, [Required] CategoryCreateUpdateDTO DTO)
        {
            ResponseBase response = _service.Update(categoryId, DTO);
            Response.StatusCode = response.Code;
            return response;
        }


    }
}
