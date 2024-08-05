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
    [Authorize]
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
            return _service.ListAll();
        }

        [HttpGet("Paged")]
        [Role(Roles.Admin)]
        public ResponseBase List(string? name, [Required] int page = 1)
        {
            return _service.ListPaged(name, page);
        }

        [HttpPost]
        [Role(Roles.Admin)]
        public ResponseBase Create([Required] CategoryCreateUpdateDTO DTO)
        {
            return _service.Create(DTO);
        }

        [HttpGet("{categoryId}")]
        [Role(Roles.Admin)]
        public ResponseBase Detail([Required] int categoryId)
        {
            return _service.Detail(categoryId);
        }

        [HttpPut("{categoryId}")]
        [Role(Roles.Admin)]
        public ResponseBase Update([Required] int categoryId, [Required] CategoryCreateUpdateDTO DTO)
        {
            return _service.Update(categoryId, DTO);
        }


    }
}
