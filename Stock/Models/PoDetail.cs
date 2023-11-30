using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Models
{
    public class PoDetail
    {
        public Guid Id { get; set; }


        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal Quantity { get; set; }


        //Fob - цена поставщика в иностранной валюте
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal Fob { get; set; }


        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal PrcInBaseCur { get; set; }


        [Required]
        [ForeignKey("PoHeader")]
        public Guid PoHeaderId { get; set; }
        public virtual PoHeader PoHeader { get; private set; }


        [Required]
        [MaxLength(6)]
        [ForeignKey("Product")]
        public string ProductCode { get; set; }
        public virtual Product Product { get; private set; }


        [StringLength(75)]
        [NotMapped]
        public string Description { get; set; } = string.Empty;
        [StringLength(25)]
        [NotMapped]
        public string UnitName { get; set; } = string.Empty;
        [NotMapped]
        public bool IsDeleted { get; set; } = false;
    }
}
