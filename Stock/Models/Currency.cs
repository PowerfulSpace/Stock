using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.Models
{
    public class Currency
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(75)]
        public string Description { get; set; } = null!;


        [ForeignKey("Currencies")]
        public Guid? ExchangeCurrencyId { get; set; }
        public virtual Currency Currencies { get; set; }

        [Column(TypeName ="smallmoney")]
        [Required]
        public decimal ExchangeRate { get; set; }
    }
}
