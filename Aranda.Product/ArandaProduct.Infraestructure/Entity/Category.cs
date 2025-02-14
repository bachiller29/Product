using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArandaProduct.Domain.Entity
{
    public class Category
    {
        [Key]
        [Column("IdCategory")]
        public int IdCategory { get; set; }

        [Column("NameCategory")]
        [Required]
        [MaxLength(100)]
        public string NameCategory { get; set; }
    }
}
