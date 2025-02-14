using ArandaProduct.Infraestructure.Entity;

namespace ArandaProduct.Infraestructure.Repositories.ProducRepository
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<bool> CategoryExistsAsync(int categoryId);
        IQueryable<Product> GetQueryableProducts();
    }
}
