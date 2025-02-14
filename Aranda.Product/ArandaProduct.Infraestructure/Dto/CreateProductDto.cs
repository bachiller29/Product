using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ArandaProduct.Domain.Dto
{
    public class CreateProductDto
    {
        [MaxLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres.")]
        [Required(ErrorMessage = "El nombre del producto es obligatoria.")]
        public string NameProduct { get; set; }

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? DescriptionProduct { get; set; }

        [Required(ErrorMessage = "La categoría del producto es obligatoria.")]
        public int IdCategory { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
