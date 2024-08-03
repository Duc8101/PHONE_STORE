using Common.Base;
using Common.DTO.ProductDTO;

namespace API.Services.Products
{
    public interface IProductService
    {
        ResponseBase List(bool isAdmin, string? name, int? categoryId, int page);
        ResponseBase Create(ProductCreateUpdateDTO DTO);
        ResponseBase Detail(Guid productId);
        ResponseBase Update(Guid productId, ProductCreateUpdateDTO DTO);
        ResponseBase Delete(Guid productId);
    }
}
