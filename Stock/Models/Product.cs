using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Models
{
    public class Product
    {
        [Key]
        [StringLength(6)]
        public string Code { get; set; } = null!;

        [Required]
        [StringLength(75)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string Description { get; set; } = null!;

        [Required]
        [Column(TypeName ="smallmoney")]
        public decimal Cost { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("Unit")]
        [Display(Name="Unit")]
        public Guid UnitId { get; set; }
        public virtual Unit Unit { get; set; } = null!;
    }
}
