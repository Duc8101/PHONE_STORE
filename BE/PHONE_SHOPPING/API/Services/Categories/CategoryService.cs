using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.DTO.CategoryDTO;
using Common.Entity;
using Common.Enums;
using Common.Paginations;
using DataAccess.DBContext;
using System.Net;

namespace API.Services.Categories
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(IMapper mapper, PHONE_STOREContext context) : base(mapper, context)
        {

        }

        public ResponseBase ListAll()
        {
            try
            {
                List<Category> list = _context.Categories.ToList();
                List<CategoryListDTO> result = _mapper.Map<List<CategoryListDTO>>(list);
                return new ResponseBase(result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        private IQueryable<Category> getQuery(string? name)
        {
            IQueryable<Category> query = _context.Categories;
            if (name != null && name.Trim().Length > 0)
            {
                query = query.Where(c => c.CategoryName.ToLower().Contains(name.Trim().ToLower()));
            }
            return query;
        }
        public ResponseBase ListPaged(string? name, int page)
        {
            try
            {
                IQueryable<Category> query = getQuery(name);
                List<Category> list = query.Skip((int)PageSize.Category_List * (page - 1)).Take((int)PageSize.Category_List)
                    .OrderByDescending(c => c.UpdateAt).ToList();
                List<CategoryListDTO> DTO = _mapper.Map<List<CategoryListDTO>>(list);
                int count = query.Count();
                int number = (int)Math.Ceiling((double)count / (int)PageSize.Category_List);
                string preURL = "/ManagerCategory";
                string nextURL = "/ManagerCategory";
                string firstURL = "/ManagerCategory";
                string lastURL = "/ManagerCategory";
                if (name == null || name.Trim().Length == 0)
                {
                    preURL = preURL + "?page=" + (page - 1);
                    nextURL = nextURL + "?page=" + (page + 1);
                    lastURL = lastURL + "?page=" + number;
                }
                else
                {
                    preURL = preURL + "?name=" + name.Trim() + "&page=" + (page - 1);
                    nextURL = nextURL + "?name=" + name.Trim() + "&page=" + (page + 1);
                    firstURL = firstURL + "?name=" + name.Trim();
                    lastURL = lastURL + "?name=" + name.Trim() + "&page=" + number;
                }
                Pagination<CategoryListDTO> data = new Pagination<CategoryListDTO>()
                {
                    PageSelected = page,
                    NEXT_URL = nextURL,
                    FIRST_URL = firstURL,
                    PRE_URL = preURL,
                    LAST_URL = lastURL,
                    NumberPage = number,
                    List = DTO
                };
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public ResponseBase Create(CategoryCreateUpdateDTO DTO)
        {
            try
            {
                if (_context.Categories.Any(c => c.CategoryName == DTO.CategoryName.Trim()))
                {
                    return new ResponseBase(false, "Category existed", (int)HttpStatusCode.Conflict);
                }
                Category category = new Category()
                {
                    CategoryName = DTO.CategoryName.Trim(),
                    CreatedAt = DateTime.Now,
                    UpdateAt = DateTime.Now,
                    IsDeleted = false,
                };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return new ResponseBase(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public ResponseBase Detail(int categoryId)
        {
            try
            {
                Category? category = _context.Categories.Find(categoryId);
                if (category == null)
                {
                    return new ResponseBase("Not found category", (int)HttpStatusCode.NotFound);
                }
                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
        public ResponseBase Update(int categoryId, CategoryCreateUpdateDTO DTO)
        {
            try
            {
                Category? category = _context.Categories.Find(categoryId);
                if (category == null)
                {
                    return new ResponseBase("Not found category", (int)HttpStatusCode.NotFound);
                }
                category.CategoryName = DTO.CategoryName.Trim();
                CategoryListDTO data = _mapper.Map<CategoryListDTO>(category);
                if (_context.Categories.Any(c => c.CategoryName == DTO.CategoryName.Trim() && c.CategoryId != categoryId))
                {
                    return new ResponseBase(data, "Category existed", (int)HttpStatusCode.Conflict);
                }
                category.UpdateAt = DateTime.Now;
                _context.Categories.Update(category);
                _context.SaveChanges();
                return new ResponseBase(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
