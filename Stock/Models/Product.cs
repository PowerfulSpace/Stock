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

        [ForeignKey("Brand")]
        [Display(Name = "Brand")]
        public Guid? BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;


        [ForeignKey("Category")]
        [Display(Name = "Category")]
        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        [ForeignKey("ProductGroup")]
        [Display(Name = "ProductGroup")]
        public Guid? ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroup { get; set; } = null!;

        [ForeignKey("ProductProfile")]
        [Display(Name = "ProductProfile")]
        public Guid? ProductProfileId { get; set; }
        public virtual ProductProfile ProductProfile { get; set; } = null!;
    }
}
