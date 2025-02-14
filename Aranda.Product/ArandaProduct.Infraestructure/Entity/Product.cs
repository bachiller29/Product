using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ArandaProduct.Domain.Entity;

namespace ArandaProduct.Infraestructure.Entity
{
    public class Product
    {
        [Key]
        [Column("IdProduct")]
        public int IdProduct { get; set; }

        [Column("NameProduct")]        
        [MaxLength(100)]
        public string NameProduct { get; set; }

        [Column("DescriptionProduct")]      
        [MaxLength(500)]
        public string? DescriptionProduct { get; set; }

        [Column("IdCategory")]
        [ForeignKey("Category")]
        public int IdCategory { get; set; }

        public Category Category { get; set; }

        [Column("ImageProduct")]
        public byte[]? ImageProduct { get; set; }      
    }
}
