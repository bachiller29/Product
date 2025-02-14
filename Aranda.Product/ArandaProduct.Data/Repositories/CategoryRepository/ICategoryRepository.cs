using ArandaProduct.Domain.Entity;

namespace ArandaProduct.Infraestructure.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsync(Category category);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
