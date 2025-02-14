using ArandaProduct.Aplication.Services.PorductService.Interface;
using ArandaProduct.Domain.Dto;
using ArandaProduct.Infraestructure.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ArandaProduct.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productsService;

        public ProductController(IProductService productsService)
        {
            _productsService = productsService;
        }

        /// <summary>
        /// Crea un nuevo producto en el sistema.
        /// </summary>
        /// <param name="createProductDto">Objeto que contiene los datos del producto a crear.</param>
        /// <returns>
        /// Devuelve un código de estado 201 (Created) con el producto creado, 
        /// un código 400 (Bad Request) si los datos son inválidos, 
        /// o un código 500 (Internal Server Error) si ocurre un error inesperado.
        /// </returns>
        /// <response code="201">Producto creado exitosamente.</response>
        /// <response code="400">Error de validación en los datos de entrada.</response>
        /// <response code="500">Error interno al procesar la solicitud.</response>
        [HttpPost("CreateProduct")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            try
            {
                var result = await _productsService.CreateProductAsync(createProductDto);
                return CreatedAtAction(nameof(GetProductByID), new { id = result.IdProduct }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno al crear el producto.");
            }
        }

        /// <summary>
        /// Actualiza un producto existente en el sistema.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="updateProductDto">Objeto con los datos a modificar del producto.</param>
        /// <returns>
        /// Devuelve un código de estado 200 (OK) si la actualización es exitosa, 
        /// un código 400 (Bad Request) si los datos son inválidos, 
        /// un código 404 (Not Found) si el producto no existe, 
        /// o un código 500 (Internal Server Error) en caso de error inesperado.
        /// </returns>
        /// <response code="200">Producto actualizado correctamente.</response>
        /// <response code="400">Error de validación en los datos de entrada.</response>
        /// <response code="404">El producto no fue encontrado.</response>
        /// <response code="500">Error interno al procesar la solicitud.</response>
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                var updatedProductId = await _productsService.UpdateProductAsync(id, updateProductDto);

                if (updatedProductId == null)
                    return NotFound("Producto no encontrado.");

                return Ok(new { Id = updatedProductId, Message = "Producto actualizado correctamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error interno al actualizar el producto.");
            }
        }

        /// <summary>
        /// Elimina un producto del sistema.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>
        /// Devuelve un código de estado 200 (OK) si la eliminación es exitosa, 
        /// un código 400 (Bad Request) si el ID no es válido, 
        /// un código 404 (Not Found) si el producto no existe, 
        /// o un código 500 (Internal Server Error) en caso de error inesperado.
        /// </returns>
        /// <response code="200">Producto eliminado correctamente.</response>
        /// <response code="400">El ID proporcionado no es válido.</response>
        /// <response code="404">El producto no fue encontrado.</response>
        /// <response code="500">Error interno al procesar la solicitud.</response>
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var deletedProductId = await _productsService.DeleteAsync(id);
                return Ok(new
                {
                    Id = deletedProductId,
                    Message = "Producto eliminado correctamente."
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Producto no encontrado.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno al eliminar el producto.");
            }
        }

        /// <summary>
        /// Obtiene una lista de productos con opciones de filtrado, ordenamiento y paginación.
        /// </summary>
        /// <param name="nameFilter">Filtro opcional por nombre del producto.</param>
        /// <param name="descriptionFilter">Filtro opcional por descripción del producto.</param>
        /// <param name="IdCategory">Filtro opcional por categoría del producto.</param>
        /// <param name="sortBy">Campo por el cual ordenar los productos (ejemplo: "NameProduct", "IdCategory").</param>
        /// <param name="ascending">Determina si el ordenamiento es ascendente (true) o descendente (false).</param>
        /// <param name="page">Número de página para la paginación (por defecto 1).</param>
        /// <param name="pageSize">Cantidad de productos por página (por defecto 10).</param>
        /// <returns>
        /// Devuelve un código de estado 200 (OK) con la lista de productos encontrados,
        /// o un código 500 (Internal Server Error) en caso de error inesperado.
        /// </returns>
        /// <response code="200">Lista de productos obtenida exitosamente.</response>
        /// <response code="500">Error interno al obtener los productos.</response>
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(
        [FromQuery] string? nameFilter = null,
        [FromQuery] string? descriptionFilter = null,
        [FromQuery] int? IdCategory = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool ascending = true,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _productsService.GetAllProductsAsync(
                    nameFilter,
                    descriptionFilter,
                    IdCategory,
                    sortBy,
                    ascending,
                    page,
                    pageSize
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno al obtener los productos.");
            }
        }

        /// <summary>
        /// Obtiene un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a obtener.</param>
        /// <returns>
        /// Devuelve un código de estado 200 (OK) con los detalles del producto si se encuentra,
        /// un código 404 (Not Found) si el producto no existe,
        /// o un código 500 (Internal Server Error) en caso de error inesperado.
        /// </returns>
        /// <response code="200">Producto encontrado y retornado exitosamente.</response>
        /// <response code="404">Producto no encontrado.</response>
        /// <response code="500">Error interno al obtener el producto.</response>
        [HttpGet("GetProductByID/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByID(int id)
        {
            try
            {
                var result = await _productsService.GetProductByIdAsync(id);
                if (result == null)
                    return NotFound(new { Message = "Producto no encontrado." });

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error interno al obtener el producto." });
            }
        }
    }
}
