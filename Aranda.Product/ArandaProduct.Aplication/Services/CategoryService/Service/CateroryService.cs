using ArandaProduct.Aplication.Services.CategoryService.Interface;
using ArandaProduct.Domain.Dto;
using ArandaProduct.Domain.Entity;
using ArandaProduct.Infraestructure.Repositories.CategoryRepository;

namespace ArandaProduct.Aplication.Services.CategoryService.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repository)
        {
            _repo = repository;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = new Category
            {
                NameCategory = createCategoryDto.NameCategory
            };

            var createdCategory = await _repo.CreateCategoryAsync(category);

            return new CategoryDto
            {
                IdCategory = createdCategory.IdCategory,
                NameCategory = createdCategory.NameCategory
            };
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryDto
            {
                IdCategory = c.IdCategory,
                NameCategory = c.NameCategory
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null) return null;

            return new CategoryDto
            {
                IdCategory = category.IdCategory,
                NameCategory = category.NameCategory
            };
        }

        public async Task<DeleteCategoryResult> DeleteCategoryAsync(int id)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null)
                return new DeleteCategoryResult { Message = "Categoría no encontrada.", IdCategory = id };

            bool deleted = await _repo.DeleteCategoryAsync(id);
            if (!deleted)
                return new DeleteCategoryResult { Message = "No se pudo eliminar la categoría.", IdCategory = id };

            return new DeleteCategoryResult { Message = "Categoría eliminada exitosamente.", IdCategory = id };
        }
    }
}
