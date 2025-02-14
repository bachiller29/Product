using ArandaProduct.Infraestructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace ArandaProduct.Infraestructure.Repositories.ProducRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FindAsync(product.IdProduct);
            if (existingProduct == null) return false;
            
            _context.Entry(existingProduct).CurrentValues.SetValues(product);            

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);

            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("No se puede eliminar el producto porque está relacionado con otros registros.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Mapeador que se utiliza para convertir una entidad de dominio (Product) en un DTO 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.IdProduct == id);
        }

        public async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.IdCategory == categoryId);
        }

        public IQueryable<Product> GetQueryableProducts()
        {
            return _context.Products.AsNoTracking();
        }
    }
}
