namespace ArandaProduct.Infraestructure.Dto
{
    public class ProductDto
    {
        public int IdProduct { get; set; }

        public string NameProduct { get; set; }

        public string DescriptionProduct { get; set; }

        public int IdCategory { get; set; }

        public string CategoryName { get; set; }

        public byte[]? ImageProduct { get; set; }        
    }
}
