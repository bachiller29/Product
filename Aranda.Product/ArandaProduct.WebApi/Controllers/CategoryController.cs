using ArandaProduct.Aplication.Services.CategoryService.Interface;
using ArandaProduct.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ArandaProduct.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="createCategoryDto">Datos de la categoría a crear.</param>
        /// <returns>
        /// Devuelve un código de estado 201 (Created) con la categoría creada,
        /// o un código 500 (Internal Server Error) en caso de error inesperado.
        /// </returns>
        /// <response code="201">Categoría creada exitosamente.</response>
        /// <response code="500">Error interno al crear la categoría.</response>
        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            var category = await _categoryService.CreateCategoryAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.IdCategory }, category);
        }

        /// <summary>
        /// Obtiene todas las categorías disponibles.
        /// </summary>
        /// <returns>
        /// Devuelve una lista de categorías.
        /// </returns>
        /// <response code="200">Lista de categorías obtenida correctamente.</response>
        /// <response code="500">Error interno al obtener las categorías.</response>
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Obtiene una categoría por su identificador único.
        /// </summary>
        /// <param name="id">El identificador de la categoría a obtener.</param>
        /// <returns>Devuelve la categoría si se encuentra.</returns>
        /// <response code="200">Categoría encontrada y devuelta correctamente.</response>
        /// <response code="404">No se encontró la categoría con el ID especificado.</response>
        /// <response code="500">Error interno al obtener la categoría.</response>
        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound("Categoría no encontrada.");
            return Ok(category);
        }

        /// <summary>
        /// Elimina una categoría por su identificador único.
        /// </summary>
        /// <param name="id">El identificador de la categoría a eliminar.</param>
        /// <returns>Devuelve un mensaje indicando el resultado de la operación.</returns>
        /// <response code="200">La categoría fue eliminada exitosamente.</response>
        /// <response code="404">No se encontró la categoría con el ID especificado.</response>
        /// <response code="500">No se pudo eliminar la categoría debido a un error interno.</response>
        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (result.Message == "Categoría no encontrada.")
                return NotFound(result);

            if (result.Message == "No se pudo eliminar la categoría.")
                return StatusCode(500, result);

            return Ok(result);
        }
    }
}
