using Microsoft.AspNetCore.Http;

namespace ArandaProduct.Domain.Dto
{
    public class UpdateProductDto
    {            
        public string? NameProduct { get; set; }

        public string? DescriptionProduct { get; set; }

        public int? IdCategory { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
