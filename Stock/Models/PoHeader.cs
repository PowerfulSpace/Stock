using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Models
{
    //Po - пивот
    public class PoHeader
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(15)]
        public string PoNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PoDate { get; set; } = DateTime.Now.Date;

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
        public DateTime QuotationDate { get; set; } = DateTime.Now.Date;

        [Required]
        [MaxLength(500)]
        public string PaymentTerms { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Remarks { get; set; } = string.Empty;

        public virtual List<PoDetail> PoDetails { get; set; } = new List<PoDetail>();


        [Required]
        [ForeignKey("Supplier")]
        public Guid SupplierId { get; set; }
        public virtual Supplier Supplier { get; private set; }


        [Required]
        [ForeignKey("BaseCurrency")]
        public Guid BaseCurrencyId { get; set; }
        public virtual Currency BaseCurrency { get; private set; }


        [Required]
        [ForeignKey("PoCurrency")]
        public Guid PoCurrencyId { get; set; }
        public virtual Currency PoCurrency { get; private set; }


    }
}
