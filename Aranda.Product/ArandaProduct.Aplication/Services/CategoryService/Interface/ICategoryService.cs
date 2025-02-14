using ArandaProduct.Domain.Dto;

namespace ArandaProduct.Aplication.Services.CategoryService.Interface
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<DeleteCategoryResult> DeleteCategoryAsync(int id);
    }
}
