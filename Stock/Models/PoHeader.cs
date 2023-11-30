using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Models
{
    public class PoHeader
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string PoNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly PoDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal ExchangeRate { get; set; }

        [Column(TypeName = "smallmoney")]
        [Required]
        public decimal DiscountPercent { get; set; }

        [Required]
        [MaxLength(15)]
        public string QuotationNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly QuotationDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string PaymentTerms { get; set; }

        [Required]
        [MaxLength(500)]
        public string Remarks { get; set; }



        [Required]
        [ForeignKey("Supplier")]
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; private set; }


        [Required]
        [ForeignKey("BaseCurrency")]
        public Guid BaseCurrencyId { get; set; }
        public virtual Currency BaseCurrency { get; private set; }


        [Required]
        [ForeignKey("PoBaseCurrency")]
        public Guid PoCurrencyId { get; set; }
        public virtual Currency PoBaseCurrency { get; private set; }


    }
}
