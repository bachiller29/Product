using ArandaProduct.Domain.Dto;
using ArandaProduct.Infraestructure.Dto;

namespace ArandaProduct.Aplication.Services.PorductService.Interface
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<int?> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync(
            string? nameFilter,
            string? descriptionFilter,
            int? IdCategory,
            string? sortBy,
            bool ascending,
            int page,
            int pageSize);
        Task<ProductDto> GetProductByIdAsync(int id);
    }
}
