using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Stock.Models
{
    public class Supplier
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(6)]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(75)]
        public string Name { get; set; } = null!;

        [Remote("IsEmailExists", "Supplier", AdditionalFields = "Id", ErrorMessage = "Email Id Already Exists")]
        [Required]
        [MaxLength(75)]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-Mail is not Valid")]
        public string EmailId { get; set; } = null!;


        [MaxLength(75)]
        public string Address { get; set; } = null!;

        [MaxLength(75)]
        public string Phone { get; set; } = null!;
    }
}
