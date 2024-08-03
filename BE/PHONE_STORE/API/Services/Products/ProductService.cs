using API.Services.Base;
using AutoMapper;
using Common.Base;
using Common.DTO.ProductDTO;
using Common.Entity;
using Common.Enums;
using Common.Paginations;
using DataAccess.DBContext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Services.Products
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IMapper mapper, PHONE_STOREContext context) : base(mapper, context)
        {

        }

        private IQueryable<Product> getQuery(string? name, int? categoryId)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category).Where(p => p.IsDeleted == false);
            if (name != null && name.Trim().Length > 0)
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(name.Trim().ToLower()) || p.Category.CategoryName.ToLower().Contains(name.Trim().ToLower()));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            return query;
        }

        public ResponseBase List(string? name, int? categoryId, int page)
        {
            try
            {
                IQueryable<Product> query = getQuery(name, categoryId);
                int count = query.Count();
                int numberPage = (int)Math.Ceiling((double)count / (int)PageSize.Product_List);
                List<Product> products = query.OrderByDescending(p => p.UpdateAt).Skip((int)PageSize.Product_List * (page - 1))
                    .Take((int)PageSize.Product_List).ToList();
                List<ProductListDTO> list = _mapper.Map<List<ProductListDTO>>(products);
                Pagination<ProductListDTO> data = new Pagination<ProductListDTO>()
                {
                    PageSelected = page,
                    List = list,
                    NumberPage = numberPage,
                };
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Create(ProductCreateUpdateDTO DTO)
        {
            try
            {
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseBase("You have to input image link", (int)HttpStatusCode.Conflict);
                }
                if (_context.Products.Any(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false))
                {
                    return new ResponseBase("Product existed", (int)HttpStatusCode.Conflict);
                }
                Product product = _mapper.Map<Product>(DTO);
                product.ProductId = Guid.NewGuid();
                product.CreatedAt = DateTime.Now;
                product.UpdateAt = DateTime.Now;
                product.IsDeleted = false;
                _context.Products.Add(product);
                _context.SaveChanges();
                return new ResponseBase(true, "Create successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(false, ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Detail(Guid productId)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase("Not found product", (int)HttpStatusCode.NotFound);
                }
                ProductListDTO data = _mapper.Map<ProductListDTO>(product);
                return new ResponseBase(data);
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Update(Guid productId, ProductCreateUpdateDTO DTO)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).Include(p => p.OrderDetails).ThenInclude(p => p.Order)
                    .FirstOrDefault(p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase("Not found product", (int)HttpStatusCode.NotFound);
                }
                List<OrderDetail> list = product.OrderDetails.ToList();
                ProductListDTO data = _mapper.Map<ProductListDTO>(product);
                if (list.Any(od => od.Order.Status == OrderStatus.Pending.ToString()))
                {
                    return new ResponseBase(data, "You can't update this product because it is being ordered", (int)HttpStatusCode.Conflict);
                }
                product.ProductName = DTO.ProductName.Trim();
                product.Image = DTO.Image.Trim();
                product.Price = DTO.Price;
                product.CategoryId = DTO.CategoryId;
                product.Quantity = DTO.Quantity;
                data = _mapper.Map<ProductListDTO>(product);
                if (DTO.ProductName.Trim().Length == 0)
                {
                    return new ResponseBase(data, "You have to input product name", (int)HttpStatusCode.Conflict);
                }
                if (DTO.Image.Trim().Length == 0)
                {
                    return new ResponseBase(data, "You have to input image link", (int)HttpStatusCode.Conflict);
                }
                if (_context.Products.Any(p => p.ProductName == DTO.ProductName.Trim() && p.IsDeleted == false && p.ProductId != productId))
                {
                    return new ResponseBase(data, "Product existed", (int)HttpStatusCode.Conflict);
                }
                product.UpdateAt = DateTime.Now;
                _context.Products.Update(product);
                _context.SaveChanges();
                return new ResponseBase(data, "Update successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        public ResponseBase Delete(Guid productId)
        {
            try
            {
                Product? product = _context.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == productId && p.IsDeleted == false);
                if (product == null)
                {
                    return new ResponseBase("Not found product", (int)HttpStatusCode.NotFound);
                }
                product.IsDeleted = true;
                _context.Products.Update(product);
                _context.SaveChanges();
                return new ResponseBase(true, "Delete successful");
            }
            catch (Exception ex)
            {
                return new ResponseBase(ex.Message + " " + ex, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
