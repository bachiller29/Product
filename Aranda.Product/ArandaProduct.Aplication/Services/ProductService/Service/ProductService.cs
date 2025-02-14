using ArandaProduct.Aplication.Services.PorductService.Interface;
using ArandaProduct.Domain.Dto;
using ArandaProduct.Infraestructure.Dto;
using ArandaProduct.Infraestructure.Entity;
using ArandaProduct.Infraestructure.Repositories.ProducRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ArandaProduct.Aplication.Services.ProductService.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repository)
        {
            _repo = repository;
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="createProductDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            try
            {
                ValidateImageFile(createProductDto.ImageFile);

                // Validar si la categoría existe
                bool categoryExists = await _repo.CategoryExistsAsync(createProductDto.IdCategory);
                if (!categoryExists)
                {
                    throw new ArgumentException("La categoría especificada no existe.");
                }

                // Convertir DTO a entidad usando MapToProduct
                var product = MapToProduct(createProductDto);

                // Procesar la imagen si existe
                if (createProductDto.ImageFile != null)
                    product.ImageProduct = await ConvertImageToByteArrayAsync(createProductDto.ImageFile);

                // Guardar en la base de datos
                var createdProduct = await _repo.CreateProductAsync(product);

                // Retornar el DTO del producto creado
                return MapToDto(createdProduct);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno al crear el producto.", ex);
            }
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="updateProductDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int?> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del producto es obligatorio y debe ser un número válido.");

            var existingProduct = await _repo.GetProductByIdAsync(id);
            if (existingProduct == null)
                return null;

            // Validar si la categoría existe si el usuario intenta actualizarla
            if (updateProductDto.IdCategory.HasValue)
            {
                bool categoryExists = await _repo.CategoryExistsAsync(updateProductDto.IdCategory.Value);
                if (!categoryExists)
                {
                    throw new ArgumentException("La categoría especificada no existe.");
                }
                existingProduct.IdCategory = updateProductDto.IdCategory.Value;
            }

            if (!string.IsNullOrEmpty(updateProductDto.NameProduct))
                existingProduct.NameProduct = updateProductDto.NameProduct;

            if (!string.IsNullOrEmpty(updateProductDto.DescriptionProduct))
                existingProduct.DescriptionProduct = updateProductDto.DescriptionProduct;

            if (updateProductDto.ImageFile is not null && updateProductDto.ImageFile.Length > 0)
            {
                existingProduct.ImageProduct = await ConvertImageToByteArrayAsync(updateProductDto.ImageFile);
            }

            bool updated = await _repo.UpdateProductAsync(existingProduct);
            return updated ? existingProduct.IdProduct : null;
        }

        /// <summary>
        /// Elimina un producto de la base de datos por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del producto es obligatorio y debe ser un número válido.");

            var product = await _repo.GetProductByIdAsync(id);

            if (product == null)
                throw new KeyNotFoundException("Producto no encontrado.");

            bool deleted = await _repo.DeleteAsync(id);
            if (!deleted)
                throw new Exception("No se pudo eliminar el producto.");

            return id;
        }

        /// <summary>
        /// Obtiene una lista de productos con filtros y paginación.
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <param name="descriptionFilter"></param>
        /// <param name="categoryFilter"></param>
        /// <param name="sortBy"></param>
        /// <param name="ascending"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(
             string? nameFilter,
             string? descriptionFilter,
             int? IdCategory,
             string? sortBy,
             bool ascending,
             int page,
             int pageSize)
        {
            try
            {
                var query = _repo.GetQueryableProducts();

                if (!string.IsNullOrEmpty(nameFilter))
                {
                    query = query.Where(p => p.NameProduct.Contains(nameFilter));
                }

                if (!string.IsNullOrEmpty(descriptionFilter))
                {
                    query = query.Where(p => p.DescriptionProduct.Contains(descriptionFilter));
                }

                if (IdCategory.HasValue)
                {
                    query = query.Where(p => p.IdCategory == IdCategory.Value);
                }
                
                int totalRecords = await query.CountAsync();

                if (totalRecords == 0)
                {                 
                    return new List<ProductDto>();
                }
                
                query = sortBy switch
                {
                    "NameProduct" => ascending ? query.OrderBy(p => p.NameProduct) : query.OrderByDescending(p => p.NameProduct),
                    "IdCategory" => ascending ? query.OrderBy(p => p.IdCategory) : query.OrderByDescending(p => p.IdCategory),
                    _ => query
                };

                var paginatedProducts = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return paginatedProducts.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error interno al obtener los productos.", ex);
            }
        }


        /// <summary>
        /// Obtiene un producto específico por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);
            if (product == null) return null;

            return MapToDto(product);
        }

        /// <summary>
        /// Valida que la imagen cumpla con los requisitos antes de guardarla.
        /// </summary>
        /// <param name="imageFile"></param>
        /// <exception cref="ArgumentException"></exception>
        private void ValidateImageFile(IFormFile? imageFile)
        {
            if (imageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(imageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    throw new ArgumentException("Formato de imagen no válido. Solo se permiten archivos JPG, PNG o GIF.");
                }

                if (imageFile.Length > 5 * 1024 * 1024) // 5 MB
                {
                    throw new ArgumentException("El tamaño de la imagen no debe superar los 5MB.");
                }
            }
        }

        /// <summary>
        /// Convierte una imagen a un array de bytes para guardar..
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        private async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile imageFile)
        {
            using var memoryStream = new MemoryStream();
            await imageFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Convierte un CreateProductDto en una entidad Product
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        private Product MapToProduct(CreateProductDto productDto)
        {
            return new Product
            {
                NameProduct = productDto.NameProduct,
                DescriptionProduct = productDto.DescriptionProduct,
                IdCategory = productDto.IdCategory
            };
        }

        /// <summary>
        /// Convierte una entidad Product en un ProductDto.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                IdProduct = product.IdProduct,
                NameProduct = product.NameProduct,
                DescriptionProduct = product.DescriptionProduct,
                IdCategory = product.IdCategory,
                CategoryName = product.Category?.NameCategory,
                ImageProduct = product.ImageProduct
            };
        }
    }
}
